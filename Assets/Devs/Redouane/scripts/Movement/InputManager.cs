using UnityEngine;
using UnityEngine.InputSystem;

public enum Masks
{
    none,
    frog,
    owl,
    mole,
    mouse,
    bat,
    cat
}

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private bool lockCursor;

    private Masks currentMask = Masks.none;
    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void DoMoving(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        playerController.Moving(moveInput); 
    } 

    public void DoJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.Jump();
        }
    }
}
