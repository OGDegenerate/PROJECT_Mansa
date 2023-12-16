using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public float jumpHeight;
    private float gravity;
    [SerializeField]
    private float gravityMultiplier;
    public float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private float _doubleJumpMultiplier = 1f;

    private bool isJumping;
    [SerializeField]
    private bool isGrounded;
    private bool _canDoubleJump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }


    void Update()
    {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }
        
        gravity = Physics.gravity.y * gravityMultiplier;
        
        MoveStuff();
        JumpStuff();

        // Grounded was never set so we didn't know when to apply some controls
        isGrounded = characterController.isGrounded;
    }

    private void MoveStuff()
    {
        // Gathering Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, maxSpeed, 5 * Time.deltaTime);
        }
        else
        {
            speed = 5;
        }

        // Movement Calculations
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();
        
        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        // PLayer Look Direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void JumpStuff()
    {
        ySpeed += gravity * Time.deltaTime;
        
        if (characterController.isGrounded)
        {
            _canDoubleJump = true;
            lastGroundedTime = Time.time;

            if (Input.GetButtonDown("Jump"))
            {
                jumpButtonPressedTime = Time.time;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && _canDoubleJump)
            {
                ySpeed = jumpHeight * _doubleJumpMultiplier;
                _canDoubleJump = false;
            }
        }

        // If the player still has a little time to jump
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -2 * gravity);
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }
        
        // Increases fall speed
        if (!isGrounded && ySpeed < 0)
        {
            ySpeed += Physics.gravity.y * 3f * Time.deltaTime;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
