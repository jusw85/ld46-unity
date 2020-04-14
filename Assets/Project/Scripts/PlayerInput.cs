using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicPlatformController))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float horizontalDeadzone = 0.2f;
    [SerializeField] private float verticalDeadzone = 0.2f;

    private Animator animator;
    private DynamicPlatformController platformController;
    private static Dictionary<string, int> paramIdMap;
    private static readonly string[] paramIds = {"running", "vSpeed", "isGrounded"};

    private bool isFacingRight = true;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        platformController = GetComponent<DynamicPlatformController>();

        if (paramIdMap == null)
        {
            paramIdMap = new Dictionary<string, int>();
            foreach (var paramId in paramIds)
            {
                paramIdMap.Add(paramId, Animator.StringToHash(paramId));
            }
        }
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
        
        animator.SetBool(paramIdMap["running"], Mathf.Abs(platformController.Velocity.x) > 0);
        animator.SetBool(paramIdMap["isGrounded"], platformController.IsGrounded);
        animator.SetFloat(paramIdMap["vSpeed"], platformController.Velocity.y);
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