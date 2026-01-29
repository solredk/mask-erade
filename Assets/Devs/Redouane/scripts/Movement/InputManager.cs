using Unity.VisualScripting;
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
    [SerializeField] private UIManager uiManager;

    [SerializeField] private bool lockCursor;

    [SerializeField] private Masks currentMask = Masks.none;
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
        if (playerController == null) { return; }

        Vector2 moveInput = context.ReadValue<Vector2>();
        playerController.Moving(moveInput, currentMask); 
    } 

    public void DoSprint(InputAction.CallbackContext context)
    {
        if (playerController == null) { return; }

        if (context.performed)
        {
            playerController.isSprinting = true;
        }
        else if (context.canceled)
        {
            playerController.isSprinting = false;
        }
    }

    public void DoJump(InputAction.CallbackContext context)
    {
        if (playerController == null) { return; }

        if (context.performed)
        {
            playerController.Jump(currentMask);
        }
    }
    public void DoPause(InputAction.CallbackContext context)
    {
        if (uiManager == null) { return; }

        if (context.performed)
        {
            uiManager.PauseGame();
        }
    }
}
