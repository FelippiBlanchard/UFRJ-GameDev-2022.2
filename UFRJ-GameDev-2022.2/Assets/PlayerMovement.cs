using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
//Variables
    //Animations
    private CharacterController controller;
    private Animator anim;

    private Rigidbody rb;
    private CapsuleCollider cc;

    //Overall
    public bool canMove = true; //referente ao controle de meios de locomoção (navio, montaria, etc)
    private float moveSpeed = 0;

    //Idle

    //Ground movement
    private float SpeedX;
    private float SpeedZ;
    private float jogSpeed = 0.02f;
    private float sprintSpeed = 0.04f;

    //Jump
    private bool canJump = true;
    private float jumpHeight = 6;

    //Swim
    private bool canSwim = false;
    private float swimSlowSpeed = 0.01f;
    private float swimFastSpeed = 0.02f;

    //Crouch
    private bool canCrouch = true;
    private float crouchSpeed = 0.01f;

    //Climb Ladder
    private bool canClimbLadder = false;
    private bool climbingLadder = false;
    private float climbLadderSpeed = 0.01f;
    private float lerpPct = 0.5f;
    private Collider ladder;

    //SteerShip


// Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

// Update is called once per frame
    void Update()
    {
        
        if (canMove){

            //Jump
            if (Input.GetKeyDown(KeyCode.Space)) {
                //Climbing Ladder
                if (canClimbLadder) {
                    ClimbLadder(ladder);
                }
                //If crouching and press space, player stands
                else if (!canCrouch && !canSwim) {
                    CrouchUp();
                }
                else if (canJump && !canClimbLadder){
                    //Jumps and calls animation
                    Jump();
                }
            }
            //Crouch
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                //Drop from ladder
                if (climbingLadder) {
                    DropLadder();
                }
                else if (canClimbLadder && !climbingLadder) {
                    ClimbLadderFromTop(ladder);
                }
                else if (canCrouch){
                    //Crouches and calls animation
                    CrouchDown();
                    CrouchIdle();
                } else if (!canSwim){ //If left control pressed when swimming, collider doubles
                    //Stands up and calls animation
                    CrouchUp();
                }
            }

            //XY movement - walking and swimming

            //Climbing
            if (climbingLadder) {
                if (Input.GetKey("w") || Input.GetKey("s")) {
                    ClimbMovement();
                } else {
                    ClimbIdle();
                }
            }
            //Swimming
            else if (canSwim){

                if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")) {
                    if (Input.GetKey(KeyCode.LeftShift)) {
                        //Se shift apertado, nada rápido
                        SwimFast();
                    } else {
                        //Sem shift, nada devagar
                        SwimSlow();
                    }
                } else {
                    //Not moving
                    SwimIdle();
                }
            }
            //Walking
            else {

                if (!canCrouch) { //If crouching
                    if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")) {
                        if (Input.GetKey(KeyCode.LeftShift)) {
                            //If shift, leaves crouch and sprints
                            CrouchUp();
                            Sprint();
                        } else {
                            //No shift crouches
                            CrouchMove();
                        }
                    } else {
                        //Not moving
                        CrouchIdle();
                    }
                } else {
                    if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")) {
                        if (Input.GetKey(KeyCode.LeftShift)) {
                            //If shift, sprints
                            Sprint();
                        } else {
                            //No shift, jogs
                            Jog();
                        }
                    } else {
                        //Not moving
                        Idle();
                    }
                }

            }

            Falling();
            Moving();
        }

    }

//Movement Functions
    //Movement
    private void Moving() {

        //Stop pivot from rotating (since pivot is child of player, it rotates)
        Camera.main.transform.GetComponent<CameraMovement>().pivot.transform.parent = Camera.main.transform;
        
        if (climbingLadder){
            rb.velocity = Vector3.zero; //If player starts climbing after falling or jumping, this stops going up or down (because of gravity fase)
            //Up
            if (Input.GetKey("w")) {
                transform.position += new Vector3(0, climbLadderSpeed, 0);
            }
            //Down
            else if (Input.GetKey("s")) {
                transform.position -= new Vector3(0, climbLadderSpeed, 0);
            }
        }
        else {
            
            Vector3 cameraForwardXZ = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            Vector3 cameraRightXZ = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
            //Forward
            if (Input.GetKey("w")) {
                transform.position += cameraForwardXZ * moveSpeed;
            }
            //Backward
            if (Input.GetKey("s")) {
                transform.position -= cameraForwardXZ * moveSpeed;
            }
            //Left
            if (Input.GetKey("a")) {
                transform.position -= cameraRightXZ * moveSpeed;
            }
            //Right
            if (Input.GetKey("d")) {
                transform.position += cameraRightXZ * moveSpeed;
            }

            if ( Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")) {
                //Rotate player in camera direction
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            }

        }
        
        //Stop pivot from rotating (since pivot is child of player, it rotates)
        Camera.main.transform.GetComponent<CameraMovement>().pivot.transform.parent = transform;
    }

    //Idle
    private void Idle() {
        anim.SetFloat("SpeedX", 0, 0.1f, Time.deltaTime);
        anim.SetFloat("SpeedZ", 0, 0.1f, Time.deltaTime);
    }
    //Jog
    private void Jog() {
        if (Input.GetKey("w")) {
            moveSpeed = jogSpeed;
            anim.SetFloat("SpeedX", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", 0.5f, 0.1f, Time.deltaTime);
        }
        //Backward
        if (Input.GetKey("s")) {
            moveSpeed = jogSpeed * 0.8f;
            anim.SetFloat("SpeedX", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", -0.5f, 0.1f, Time.deltaTime);
        }
        //Left
        if (Input.GetKey("a")) {
            moveSpeed = jogSpeed * 0.8f;
            anim.SetFloat("SpeedX", -0.5f, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", 0, 0.1f, Time.deltaTime);
        }
        //Right
        if (Input.GetKey("d")) {
            moveSpeed = jogSpeed * 0.8f;
            anim.SetFloat("SpeedX", 0.5f, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", 0, 0.1f, Time.deltaTime);
        }
    }
    //Sprint
    private void Sprint() {
        if (Input.GetKey("w")) {
            moveSpeed = sprintSpeed;
            anim.SetFloat("SpeedX", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", 1, 0.1f, Time.deltaTime);
        }
        //Backward
        if (Input.GetKey("s")) {
            moveSpeed = sprintSpeed * 0.8f;
            anim.SetFloat("SpeedX", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", -1, 0.1f, Time.deltaTime);
        }
        //Left
        if (Input.GetKey("a")) {
            moveSpeed = sprintSpeed;
            anim.SetFloat("SpeedX", -1, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", 0, 0.1f, Time.deltaTime);
        }
        //Right
        if (Input.GetKey("d")) {
            moveSpeed = sprintSpeed;
            anim.SetFloat("SpeedX", 1, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedZ", 0, 0.1f, Time.deltaTime);
        }
    }

    //SwimIdle
    private void SwimIdle() {
        anim.SetFloat("SwimSpeed", 0, 0.1f, Time.deltaTime);
    }
    //SwimSlow
    private void SwimSlow() {
        moveSpeed = swimSlowSpeed;
        anim.SetFloat("SwimSpeed", 0.5f, 0.1f, Time.deltaTime);
    }
    //SwimFast
    private void SwimFast() {
        moveSpeed = swimFastSpeed;
        anim.SetFloat("SwimSpeed", 1, 0.1f, Time.deltaTime);
    }

    private void SwimCollider(bool swimming) {
        if (swimming && canSwim) {
            cc.direction = 2;
        }else {
            cc.direction = 1;
        }
    }

    //Jump
    private void Jump() {
        anim.SetTrigger("Jump");
        Invoke("JumpAction", 0.5f);
    }

    private void JumpAction() {
        rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        canJump = false;
        cc.height = cc.height*0.66f;
        cc.center = new Vector3(cc.center.x, (cc.center.y)*1.37f, cc.center.z);
    }

    //Falling
    private void Falling() {
        if (rb.velocity.y < -3f) {
            anim.SetBool("Fall", true);
        } else {
            anim.SetBool("Fall", false);
        }
    }

    //Crouch
    private void CrouchIdle() {
        anim.SetFloat("CrouchSpeedX", 0, 0.1f, Time.deltaTime);
        anim.SetFloat("CrouchSpeedZ", 0, 0.1f, Time.deltaTime);
    }
    private void CrouchMove() {
        if (Input.GetKey("w")) {
            moveSpeed = crouchSpeed;
            anim.SetFloat("CrouchSpeedX", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("CrouchSpeedZ", 1, 0.1f, Time.deltaTime);
        }
        //Backward
        if (Input.GetKey("s")) {
            moveSpeed = crouchSpeed * 0.8f;
            anim.SetFloat("CrouchSpeedX", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("CrouchSpeedZ", -1, 0.1f, Time.deltaTime);
        }
        //Left
        if (Input.GetKey("a")) {
            moveSpeed = crouchSpeed;
            anim.SetFloat("CrouchSpeedX", -1, 0.1f, Time.deltaTime);
            anim.SetFloat("CrouchSpeedZ", 0, 0.1f, Time.deltaTime);
        }
        //Right
        if (Input.GetKey("d")) {
            moveSpeed = crouchSpeed;
            anim.SetFloat("CrouchSpeedX", 1, 0.1f, Time.deltaTime);
            anim.SetFloat("CrouchSpeedZ", 0, 0.1f, Time.deltaTime);
        }
    }
    private void CrouchDown() {
        cc.height = cc.height/2;
        cc.center = new Vector3(cc.center.x, (cc.center.y)/2, cc.center.z);
        canCrouch = false;
        anim.SetBool("CrouchBool", true);
    }
    private void CrouchUp() { //Stand up from crouching
        cc.height = cc.height*2;
        cc.center = new Vector3(cc.center.x, (cc.center.y)*2, cc.center.z);
        canCrouch = true;
        anim.SetBool("CrouchBool", false);
    }

    //Climb ladder
    private void ClimbIdle() {
        anim.SetFloat("ClimbingSpeed", 0, 0.1f, Time.deltaTime);
    }
    private void ClimbMovement() {
        anim.SetFloat("ClimbingSpeed", 1, 0.1f, Time.deltaTime);
    }
    private void ClimbLadder(Collider ladder) {
        climbingLadder = true;

        //Disables player gravity to climb up and down
        rb.useGravity = false;

        //Adjust player position to climb ladder
        transform.position = new Vector3(ladder.transform.position.x, transform.position.y + 0.5f, ladder.transform.position.z);

        //Adjust player rotation to match ladder position
        transform.LookAt(new Vector3(ladder.transform.parent.transform.position.x, transform.position.y, ladder.transform.parent.transform.position.z));

        //Climb ladder animation
        anim.SetBool("ClimbingLadder", true);
    }
    private void LadderTop(Vector3 startPoint) {
        //Blocks effect if player is comming from above and doesn't want to climb
        if (climbingLadder) {
            //Climbing to top of the ladder animation
            anim.SetBool("ClimbingLadder", false);

            //transform.position frente e em cima
            //Up
            Vector3 middlePoint = startPoint + new Vector3(0, 3, 0);
            transform.position = Vector3.Lerp(startPoint, middlePoint, lerpPct);
            //Forward
            transform.Translate(Vector3.forward);

            climbingLadder = false;
            rb.useGravity = true;
        }
    }
    //Leaves from ladder if reaches bottom or if press control to drop
    private void DropLadder() {
        climbingLadder = false;
        rb.useGravity = true;
        anim.SetBool("ClimbingLadder", false);
        
    }
    private void ClimbLadderFromTop(Collider ladder) {
        //Rotate player towards ladder
        transform.LookAt(new Vector3(ladder.transform.parent.transform.position.x, transform.position.y, ladder.transform.parent.transform.position.z));

        //transform.position backwards and below
        transform.position = new Vector3(
            ladder.transform.parent.Find("LadderTrigger").transform.position.x,
            transform.position.y - 2,
            ladder.transform.parent.Find("LadderTrigger").transform.position.z
            );
        
        climbingLadder = true;
        rb.useGravity = false;

        //Climb ladder from top animation (reverse of LadderTop anim)
        anim.SetBool("ClimbingLadder", true);
    }
    

//Collision Functions
    void OnTriggerExit(Collider obj) {
        if (obj.tag == "Ladder" || obj.tag == "LadderGoDown") {
            canClimbLadder = false;
        }
    }

    void OnTriggerEnter(Collider obj) {
        //SteerShip
        if (obj.tag == "SteerShip") {
            //adicionar animação de timoneiro
            obj.transform.parent.gameObject.GetComponent<ControlShipMovement>().controlar = true;
        }
        //Climb ladder
        else if (obj.tag == "Ladder" || obj.tag == "LadderGoDown") {
            canClimbLadder = true;
            ladder = obj;
        }
        //Finish climbing ladder on the top
        else if (obj.tag == "LadderTop") {
            LadderTop(transform.position);
        }
        //Finish climbing ladder on the bottom
        else if (obj.tag == "LadderBottom" && climbingLadder) {
            DropLadder();
        }
    }

    //Jump, Swim and Crouch validations
    void OnCollisionEnter(Collision obj) {
        if (obj.gameObject.tag == "Water") { //If swimmming
            canJump = false;
            canCrouch = false;
            canSwim = true;
            anim.SetTrigger("Swim");
            anim.SetBool("SwimBool", true);
        }
        else {
            if (canJump == false && canSwim == false){
                cc.height = cc.height/0.66f;
                cc.center = new Vector3(cc.center.x, (cc.center.y)/1.37f, cc.center.z);
            }
            canJump = true;
            canCrouch = true;
            canSwim = false;
            anim.SetBool("SwimBool", false);
        }
    }

}


