using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 input;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float BaseJumpHeight = 2f;
    [SerializeField] private float frogJumpHeight = 4f;
    [SerializeField] private bool shouldFaceMoveDirection = false;

    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2.0f;
    [SerializeField] private float fallAnimThreshold = -0.1f; 

    private bool jumpHeld;
    private Masks currentMask;

    private Vector3 moveDirection;
    private float verticalVelocity;
    private float jumpHeight;

    public bool isSprinting = false;
    public bool isWalking = false;

    private void Update()
    {
        animator.SetBool("Walking", isWalking);
        animator.SetBool("Sprinting", isSprinting);
        animator.SetBool("Grounded", controller.isGrounded);

        if (currentMask != Masks.frog)
        {
            jumpHeight = BaseJumpHeight;
        }
        else
        {
            jumpHeight = frogJumpHeight;
        }

        // sprint speed
        if (isSprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();


        moveDirection = (forward * input.y + right * input.x).normalized;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
            bool isFalling = !controller.isGrounded && verticalVelocity < fallAnimThreshold;
            animator.SetTrigger("Falling");
        }

        float gravityMultiplier = 1f;

        if (verticalVelocity < 0f)
        {
            gravityMultiplier = fallMultiplier;
        }

        else if (verticalVelocity > 0f && !jumpHeld)
        {
            gravityMultiplier = lowJumpMultiplier;
        }

        verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;


        Vector3 velocity = moveDirection * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);


        if (shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    public void Moving(Vector2 moveInput, Masks mask)
    {
        input = moveInput;
        currentMask = mask;
    }

    public void Jump(Masks mask)
    {
        currentMask = mask;

        if (controller.isGrounded)
        {
            StartCoroutine(JumpDelay());
        }
    }

    private IEnumerator JumpDelay()
    {
        animator.SetTrigger("Jumping");

        yield return new WaitForSeconds(0.3f);

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void SetJumpHeld(bool held)
    {
        jumpHeld = held;
    }
}