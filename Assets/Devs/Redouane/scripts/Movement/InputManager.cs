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
        if (currentMask == Masks.owl)
        {
            if (context.performed)
                playerController.isGliding = true;
            if (context.canceled)
                playerController.isGliding = false;
        }
        else
        {

            if (context.performed)
                playerController.SetJumpHeld(true);

            if (context.canceled)
                playerController.SetJumpHeld(false);

            if (context.performed)
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
    public void OnMask(InputAction.CallbackContext context)
    {

        switch (context.action.name)
        {
            case "NoneMask":
                Debug.Log("Mask changed to: " + context.action.name);
                currentMask = Masks.none; 
                break;
            case "FrogMask":
                Debug.Log("Mask changed to: " + context.action.name);
                currentMask = Masks.frog; 
                break;
            case "OwlMask": 
                currentMask = Masks.owl; 
                break;
            case "MoleMask": 
                currentMask = Masks.mole; 
                break;
            case "MouseMask": 
                currentMask = Masks.mouse; 
                break;
            case "BatMask": 
                currentMask = Masks.bat; 
                break;
            case "CatMask": 
                currentMask = Masks.cat; 
                break;
            default:
                Debug.Log("GEEN CASE GEVONDEN VOOR: " + context.action.name);
                break;
        }
    }
}
