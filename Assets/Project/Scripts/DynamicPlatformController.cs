using System;
using Jusw85.Common;
using UnityEngine;

[RequireComponent(typeof(Raycaster))]
public class DynamicPlatformController : MonoBehaviour
{
    #region Member Variables

    [SerializeField] private float walkVelocity = 8;
    [SerializeField] private float jumpVelocity = 20;
    [SerializeField] private Vector2 groundCheckProtrusion = new Vector2(0, -0.05f);
    [SerializeField] private float earlyJumpTimeTolerance = 0.1f;
    [SerializeField] private float lateJumpTimeTolerance = 0.1f;
    public event Action IsJumpingThisFrameCallback;
    public event Action IsLandingThisFrameCallback;

    #endregion

    #region Cached Variables

    private Rigidbody2D rb2d;
    private Raycaster raycaster;

    #endregion

    #region Internal State Variables

    private float walkDir;
    private Vector2 velocity;
    private bool isGrounded;
    private int jumpCount;
    private float jumpActuatedTime = -1f;
    private float touchedGroundTime = -1f;
    private float leftGroundTime = -1f;
    private bool isJumpingThisFrame;

    #endregion

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<Raycaster>();
    }

    /// <summary>
    /// TODO: Consider when to reset frame variables
    /// Use case: Multiple FixedUpdates over a single Update, or vice versa
    /// https://docs.unity3d.com/Manual/ExecutionOrder.html
    /// </summary>
    private void FixedUpdate()
    {
        // Update internal state
        Raycaster.CollisionInfo collisions = raycaster.Collide(groundCheckProtrusion);
        bool prevIsGrounded = isGrounded;
        isGrounded = collisions.below;
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (prevIsGrounded && !isGrounded)
        {
            leftGroundTime = Time.time;
        }
        else if (!prevIsGrounded && isGrounded)
        {
            touchedGroundTime = Time.time;
            IsLandingThisFrameCallback?.Invoke();
        }

        // Update velocity
        velocity = rb2d.velocity;
        velocity.x = walkDir * walkVelocity;
        if (isJumpingThisFrame)
        {
            if (isGrounded)
            {
                DoJump();
            }
            else if (jumpCount < 1 &&
                     jumpActuatedTime - leftGroundTime <= lateJumpTimeTolerance)
            {
                DoJump();
            }
        }
        else if (isGrounded && jumpActuatedTime > 0 &&
                 touchedGroundTime - jumpActuatedTime <= earlyJumpTimeTolerance)
        {
            DoJump();
        }

        rb2d.velocity = velocity;

        // Reset transient frame variables
        isJumpingThisFrame = false;
        return;

        void DoJump()
        {
            jumpCount++;
            velocity.y = jumpVelocity;
            jumpActuatedTime = -1f;
            IsJumpingThisFrameCallback?.Invoke();
        }
    }

    #region Public Methods

    public void Walk(float walkDir)
    {
        this.walkDir = Mathf.Clamp(walkDir, -1f, 1f);
    }

    public void Jump()
    {
        isJumpingThisFrame = true;
        jumpActuatedTime = Time.time;
    }

    public float WalkDir => walkDir;

    public bool IsGrounded => isGrounded;

    public Vector2 Velocity => velocity;

    #endregion
}