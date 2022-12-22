using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

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
    [SerializeField] private float jogSpeed = 0.1f;
    [SerializeField] private float sprintSpeed = 0.2f;

    //Crouch
    private bool canCrouch = true;
    [SerializeField] private float crouchSpeed = 0.05f;

    //Events
    public UnityEvent onPlayerCrouch;
    public UnityEvent onPlayerJog;
    public UnityEvent onPlayerSprint;
    public UnityEvent onPlayerIdle;


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
                //If crouching and press space, player stands
                if (!canCrouch) {
                    CrouchUp();
                }
            }
            //Crouch
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                if (canCrouch){
                    //Crouches and calls animation
                    CrouchDown();
                    CrouchIdle();
                } else if (!canCrouch){ //if already crouching
                    //Stands up and calls animation
                    CrouchUp();
                }
            }

            //XY movement - walking and swimming

            //Walking
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

            Moving();
        }

    }

//Movement Functions
    //Movement
    private void Moving() {

        //Stop pivot from rotating (since pivot is child of player, it rotates)
        Camera.main.transform.GetComponent<CameraMovement>().pivot.transform.parent = Camera.main.transform;
        
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

        //Stop pivot from rotating (since pivot is child of player, it rotates)
        Camera.main.transform.GetComponent<CameraMovement>().pivot.transform.parent = transform;
    }

    //Idle
    private void Idle() {
        anim.SetFloat("SpeedX", 0, 0.1f, Time.deltaTime);
        anim.SetFloat("SpeedZ", 0, 0.1f, Time.deltaTime);

        onPlayerIdle.Invoke();
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

        onPlayerJog.Invoke();
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

        onPlayerSprint.Invoke();
    }

    //Crouch
    private void CrouchIdle() {
        anim.SetFloat("CrouchSpeedX", 0, 0.1f, Time.deltaTime);
        anim.SetFloat("CrouchSpeedZ", 0, 0.1f, Time.deltaTime);

        onPlayerIdle.Invoke();
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

        onPlayerCrouch.Invoke();
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

//Collision Functions


}


