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

    private bool isPaused = false;

    [SerializeField] private bool lockCursor;

    [SerializeField] private Masks currentMask = Masks.none;

    [SerializeField] private GameObject PauseCanvas;
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
        playerController.Moving(moveInput, currentMask); 
    } 

    public void DoSprint(InputAction.CallbackContext context)
    {
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
        if (context.performed)
        {
            playerController.Jump(currentMask);
        }
    }


    public void DoPause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            isPaused = false;
            PauseCanvas.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isPaused = true;
            PauseCanvas.SetActive(true);
        }
    }
}
