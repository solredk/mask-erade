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

    public Masks currentMask = Masks.none;
    private void Start()
    {
        if (lockCursor)
        {
            Cursor.visible = false;
        }
    }

    public void DoMoving(InputAction.CallbackContext context)
    {
        if (playerController == null) return;

        if (context.performed)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            playerController.Moving(moveInput, currentMask);
            playerController.isWalking = true;
        }
        else if (context.canceled)
        {
            playerController.Moving(Vector2.zero, currentMask);
            playerController.isWalking = false;
        }
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
        if (playerController == null) return;

        if (context.performed)
            playerController.SetJumpHeld(true);

        if (context.canceled)
            playerController.SetJumpHeld(false);

        if (context.performed)
            playerController.Jump(currentMask);
    }


    public void DoPause(InputAction.CallbackContext context)
    {
        if (uiManager == null) { return; }

        if (context.performed)
        {
            uiManager.PauseGame();
        }
    }

    public void DoNoneMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.none;
    }

    public void DoFrogMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.frog;
    }

    public void DoOwlMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.owl;
    }

    public void DoMoleMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.mole;
    }

    public void DoMouseMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.mouse;
    }
    public void DoBatMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.bat;
    }

    public void DoCatMask(InputAction.CallbackContext context)
    {
        currentMask = Masks.cat;
    }

}
