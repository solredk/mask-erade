using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Masks
{
    MaskNone,
    FrogMask,
    MouseMask,
    BatMask,
    CatMask
}
public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CharacterController controller;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private GameObject owlMask;
    [SerializeField] private GameObject noMask;

    private bool canChangeForm;

    [SerializeField] private bool lockCursor;

    public Masks currentMask = Masks.MaskNone;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.visible = false;
        }
    }
    private void Update()
    {
        playerController.currentMask = currentMask;
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
        if (currentMask == Masks.BatMask)
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
        if (currentMask.ToString() == context.action.name )
            return;
        switch (context.action.name)
        {
            case "MaskNone":
                Debug.Log("Mask changed to: " + context.action.name);
                currentMask = Masks.MaskNone; 
                break;
            case "FrogMask":
                Debug.Log("Mask changed to: " + context.action.name);
                currentMask = Masks.FrogMask; 
                break;
            case "MouseMask": 
                currentMask = Masks.MouseMask; 
                break;
            case "BatMask": 
                currentMask = Masks.BatMask; 
                break;
            case "CatMask": 
                currentMask = Masks.CatMask; 
                break;
            default:
                Debug.Log("GEEN CASE GEVONDEN VOOR: " + context.action.name);
                break;
        }
        if(canChangeForm)
        ChangeForm();

        StartCoroutine(ChangeFormDelay());
    }
    private IEnumerator ChangeFormDelay()
    {
        canChangeForm = false;
        yield return new WaitForSeconds(1);
        canChangeForm = true;
    }
    private void ChangeForm()
    {
        if (currentMask == Masks.MaskNone)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            controller.height = 3.8f;
            controller.center = new Vector3(0, 0, 0);
            owlMask.SetActive(false);
            noMask.SetActive(true);
        }
        else if (currentMask == Masks.BatMask)
        {
            controller.height = 2f;
            controller.center = new Vector3(0, 1.1f, 0);
            owlMask.SetActive(true);
            noMask.SetActive(false);
        }
    }


}
