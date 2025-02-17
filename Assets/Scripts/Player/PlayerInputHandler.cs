using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool PausePressed { get; private set; }

    private InputAction moveAction, jumpAction, lookAction, crouchAction, interactAction, sprintAction, pauseAction;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        pauseAction = InputSystem.actions.FindAction("Cancel");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        interactAction = InputSystem.actions.FindAction("Interact");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        LookInput = lookAction.ReadValue<Vector2>();
        JumpPressed = jumpAction.IsPressed();
        CrouchPressed = crouchAction.IsPressed();
        SprintPressed = sprintAction.IsPressed();
        InteractPressed = interactAction.IsPressed();
        PausePressed = pauseAction.triggered;
    }
}
