using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private enum MovementState
    {
        gliding,
        climbing,
    }

    private Vector2 input;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float BaseJumpHeight = 2f;
    [SerializeField] private float frogJumpHeight = 4;
    private float jumpHeight = 2f;

    [SerializeField] private float gravity = -9.81f;

    private Masks currentMask;

    private Vector3 moveDirection;

    private float verticalVelocity;
    public bool isSprinting = false;


    private void Update()
    {
        Move();
        if (currentMask == Masks.frog)
        {
            jumpHeight = frogJumpHeight;
        }
        else if (currentMask != Masks.frog)
        {
            jumpHeight = BaseJumpHeight;
        }

        if (isSprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }
    }

    private void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = (right * input.x + forward * input.y).normalized;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMove = (moveDirection * speed) + (Vector3.up * verticalVelocity);
        controller.Move(finalMove * Time.deltaTime);
    }

    public void Moving(Vector2 moveInput, Masks mask)
    {
        input = moveInput;
        currentMask = mask;

        if (currentMask == Masks.frog)
        {
            jumpHeight *= 2;
        }
    }

    public void Jump(Masks mask)
    {
        currentMask = mask;
        if (controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void OnDisable()
    {
      
    }

}