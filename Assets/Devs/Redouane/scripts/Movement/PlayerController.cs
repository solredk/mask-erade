using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Main stats")]
    private Vector2 input;

    [Header("walking stats")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    private float currentSpeed = 5f;

    [SerializeField] private bool shouldFaceMoveDirection = false;

    public bool isSprinting = false;
    public bool isWalking = false;
    public bool isGliding = false;

    public Masks currentMask;

    private Vector3 moveDirection;
    private float verticalVelocity;

    [Header("jumping stats")]
    [SerializeField] private float BaseJumpHeight = 2f;
    [SerializeField] private float frogJumpHeight = 4f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float glidingGravity = -2.0f;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2.0f;
    [SerializeField] private float fallAnimThreshold = -0.1f;
    private float jumpHeight;
    private bool jumpHeld;

    [Header("Gliding stats")]
    [SerializeField] private float glideFallSpeed = -4f;   
    [SerializeField] private float glideSmooth = 8f;       
    [SerializeField] private float maxFallSpeed = -20f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Animator")]
    [SerializeField] private Animator baseAnimator;
    [SerializeField] private Animator batAnimator;

    [SerializeField] private CharacterController controller;

    private void Update()
    {
        UpdateAnimatorParameters();

        UpdateMaskStats();
   

        if (isSprinting)
        {
            currentSpeed = 8f;
        }
        else
        {
            currentSpeed = 5f;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        if (!controller.isGrounded)
        {
            batAnimator.SetBool("Gliding", isGliding);
        }

        moveDirection = (forward * input.y + right * input.x).normalized;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
            bool isFalling = !controller.isGrounded && verticalVelocity < fallAnimThreshold;
            baseAnimator.SetTrigger("Falling");
        }

        if (isGliding && !controller.isGrounded && verticalVelocity < 0f)
        {
            // gliding: forceer een zachte daalsnelheid
            verticalVelocity = Mathf.Lerp(verticalVelocity, glideFallSpeed, glideSmooth * Time.deltaTime);
        }
        else
        {
            // normale gravity
            float gravityMultiplier = 1f;

            if (verticalVelocity < 0f)
                gravityMultiplier = fallMultiplier;
            else if (verticalVelocity > 0f && !jumpHeld)
                gravityMultiplier = lowJumpMultiplier;

            verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;

            // normale terminal velocity
            verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
        }

        if (controller.isGrounded)
        {
            isGliding = false;
        }

        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);


        if (shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }



    private void UpdateAnimatorParameters()
    {
        if (currentMask == Masks.BatMask)
        {
            Debug.Log("Bat Animator Updated");
            batAnimator.SetBool("Walking", isWalking);
            if (isWalking && isSprinting)
            {
                batAnimator.SetBool("Sprinting", true);
            }
            else if (!isWalking || !isSprinting)
            {
                batAnimator.SetBool("Sprinting", false);
            }
            batAnimator.SetBool("Grounded", controller.isGrounded);
        }

       
        if (currentMask == Masks.MaskNone)
        {
            baseAnimator.SetBool("Walking", isWalking);
            if(isWalking && isSprinting)
            {
            baseAnimator.SetBool("Sprinting", true);
            }
            else if (!isWalking || !isSprinting)
            {
                baseAnimator.SetBool("Sprinting", false);
            }
            baseAnimator.SetBool("Grounded", controller.isGrounded);
        }


    }

    private void UpdateMaskStats()
    {
        if (currentMask != Masks.FrogMask)
            jumpHeight = BaseJumpHeight;
        else
            jumpHeight = frogJumpHeight;
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
        baseAnimator.SetTrigger("Jumping");

        yield return new WaitForSeconds(0.3f);

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void SetJumpHeld(bool held)
    {
        jumpHeld = held;
    }
}