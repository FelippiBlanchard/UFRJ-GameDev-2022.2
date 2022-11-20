using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public CharacterController controller;
    public Camera playerCamera;

    [Header("Controls")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    
    [Header("Movement Parameters")]
    public float speed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintSpeed = 16f; 
    public float crouchSpeed = 4f;
    
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;

    [Header("Crouching Stuff")]
    private float crouchingHeight = 0.5f;
    private float standingHeight = 2f;
    private float timeToCrouch = 0.25f;
    private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching = false;
    private bool duringCrouchAnimation = false;

    Vector3 velocity;
    bool isGrounded;
    private bool isSprinting => Input.GetKey(sprintKey);
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        HandleGravity();
        // HandleJump();
        HandleMovement();
        HandleCrouch();
    }

    private void HandleMovement() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        float currentSpeed = speed;
        if(isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        if(isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        if(isSprinting && isCrouching && !duringCrouchAnimation)
        {
            StartCoroutine(CrouchStand());
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleGravity() {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump() {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleCrouch() {
        if(Input.GetKeyDown(crouchKey) && !duringCrouchAnimation)
            StartCoroutine(CrouchStand());
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;
        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchingHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        while(timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed/timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed/timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;
        duringCrouchAnimation = false;
    }
    
}
