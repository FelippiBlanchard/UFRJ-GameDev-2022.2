using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
//Variables
    //Animations
    private CharacterController controller;
    private Animator anim;
    private Rigidbody rb;
    private CapsuleCollider cc;

    //Overall
    

    //Roll
    private float dodgeForwardDistance = 6f;
    private float dodgeBackDistance = 4f;
    private float dodgeSideDistance = 5f;

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
        //Dodge
        if (Input.GetKeyDown("c") && ( Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d") ) ) {
            Debug.Log("dodge");
            dodgeMovement();
        }
    }

    void dodgeMovement() {
        gameObject.GetComponent<PlayerMovement>().canMove = false;
        cc.height = cc.height/2;
        cc.center = new Vector3(cc.center.x, (cc.center.y)/2, cc.center.z);
        
        //Forward
        if (Input.GetKey("w")) {
            rb.velocity = transform.forward * dodgeForwardDistance;
            anim.SetTrigger("DodgeForward");
        }
        //Backward
        if (Input.GetKey("s")) {
            rb.velocity = -transform.forward * dodgeBackDistance;
            anim.SetTrigger("DodgeBackward");
        }
        //Left
        if (Input.GetKey("a")) {
            rb.velocity = -transform.right * dodgeSideDistance;
            anim.SetTrigger("DodgeLeft");
        }
        //Right
        if (Input.GetKey("d")) {
            rb.velocity = transform.right * dodgeSideDistance;
            anim.SetTrigger("DodgeRight");
        }

        cc.height = cc.height*2;
        cc.center = new Vector3(cc.center.x, (cc.center.y)*2, cc.center.z);
        gameObject.GetComponent<PlayerMovement>().canMove = true;
    }
}
