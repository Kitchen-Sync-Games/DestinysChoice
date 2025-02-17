using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded, isJumping, isCrouching;
    private float xRotation = 0f, baseCharacterHeight, speed;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float baseSpeed = 2.6f;
    [SerializeField] private float jumpPower = 1.0f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float mouseSensitivity = 0.1f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        baseCharacterHeight = controller.height;
        speed = baseSpeed;
    }

    public void HandleLook(Vector2 lookValue,bool paused){
        //camera
        if(!paused)
            transform.Rotate(lookValue.x * mouseSensitivity * Vector3.up);
            xRotation -= lookValue.y * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void HandleMovement(Vector2 moveInput)
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            isJumping = false;
        }

        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        controller.Move(speed * Time.deltaTime * moveDirection);
    }

    public void HandleJump(bool jumpPressed)
    {
        if (jumpPressed && isGrounded && !isCrouching)
        {
            playerVelocity.y = Mathf.Sqrt(jumpPower * -2.0f * gravity);
            isJumping = true;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void HandleCrouch(bool crouchPressed)
    {
        if (crouchPressed && !isJumping && playerVelocity.y <= 0)
        {
            isCrouching = true;
            controller.height = baseCharacterHeight * 0.5f;
            controller.center = new Vector3(0, -baseCharacterHeight / 6.0f, 0);
            speed = baseSpeed / 2;
        }
        else if (!crouchPressed)
        {
            isCrouching = false;
            controller.height = baseCharacterHeight;
            controller.center = Vector3.zero;
            speed = baseSpeed;
        }
    }

    //********* Old Classes for reference ***********

// void RotateAndMovePlayer(Vector2 moveValue, Vector2 lookValue)
//     {
//         if (!paused && !cutScene)
//         {
//             // Check if grounded
//             isGrounded = controller.isGrounded;
//             if (isGrounded && playerVelocity.y < 0)
//             {
//                 playerVelocity.y = 0f;
//                 isJumping = false;
//             }

//             //camera
//             // transform.Rotate(lookValue.x * mouseSensitivity * Vector3.up);
//             // xRotation -= lookValue.y * mouseSensitivity;
//             xRotation = Mathf.Clamp(xRotation, -60f, 60f);
//             cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

//             // Movement
//             Vector3 moveDirection = (transform.forward * moveValue.y + transform.right * moveValue.x).normalized;
//             if (moveValue.y == 0 && moveValue.x == 0)
//             {
//                 if (noises[0].isPlaying)
//                     noises[0].Stop();
//             }
//             else
//             {
//                 if (!noises[0].isPlaying)
//                     noises[0].Play();
//             }
//             controller.Move(speed * Time.deltaTime * moveDirection);

//             // Jumping
//             if (inputHandler.JumpPressed && isGrounded && !isCrouching)
//             {
//                 playerVelocity.y += Mathf.Sqrt(jumpPower * -2.0f * gravity);
//                 isJumping = true;
//             }
//             playerVelocity.y += gravity * Time.deltaTime;
//             controller.Move(playerVelocity * Time.deltaTime); 
//         }   
//         else if (!paused)
//         {
//             //cutscene camera
//             // yRotationForCutscene += lookValue.x * mouseSensitivity;
//             // yRotationForCutscene = Mathf.Clamp(yRotationForCutscene, -90f, 90f);
//             // xRotation -= lookValue.y * mouseSensitivity;
//             xRotation = Mathf.Clamp(xRotation, -60f, 60f);
//             cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotationForCutscene, 0f);
//         }
//     }
    
    // Sprinting (no sprinting until later probably)
    // void OnSprintPressed(InputAction.CallbackContext context)
    // {
    //     if (!isCrouching)
    //     {
    //         speed = baseSpeed * 1.5f;
    //     }
    // }
    // void OnSprintReleased(InputAction.CallbackContext context)
    // {
    //     speed = baseSpeed;
    // }
}
