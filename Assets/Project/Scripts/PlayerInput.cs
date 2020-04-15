using System.Collections.Generic;
using k;
using UnityEngine;

[RequireComponent(typeof(DynamicPlatformController))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float horizontalDeadzone = 0.2f;
    [SerializeField] private float verticalDeadzone = 0.2f;

    private Animator animator;
    private DynamicPlatformController platformController;

    private bool isFacingRight = true;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        platformController = GetComponent<DynamicPlatformController>();
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Mathf.Abs(moveInput.x) > horizontalDeadzone)
        {
            float walkDir = Mathf.Sign(moveInput.x) > 0 ? 1 : -1;
            platformController.Walk(walkDir);
            SetIsFacingRight(walkDir > 0);
        }
        else
        {
            platformController.Walk(0);
        }

        bool upKeyPressed = moveInput.y > verticalDeadzone;
        bool jumpKeyPressed = Input.GetButtonDown("Jump");
        bool jumpPressed = jumpKeyPressed || upKeyPressed;
        if (jumpPressed)
        {
            platformController.Jump();
        }
        
        animator.SetBool(AnimatorParams.RUNNING, Mathf.Abs(platformController.Velocity.x) > 0);
        animator.SetBool(AnimatorParams.IS_GROUNDED, platformController.IsGrounded);
        animator.SetFloat(AnimatorParams.V_SPEED, platformController.Velocity.y);
    }

    private void SetIsFacingRight(bool isFacingRight)
    {
        if (this.isFacingRight ^ isFacingRight)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    
        this.isFacingRight = isFacingRight;
    }
}