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
    [SerializeField] private float horizontalDeadzone = 0.2f;
    [SerializeField] private float verticalDeadzone = 0.2f;

    #endregion

    #region Cached Variables

    private Rigidbody2D rb2d;
    private Raycaster raycaster;
    private float dft;

    #endregion

    #region Internal State Variables

    private FrameInfo frameInfo;
    private bool isGrounded;
    private int jumpCount;
    private float earlyJumpCountdownTimer = -1f;
    private float lateJumpCountdownTimer = -1f;

    #endregion

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<Raycaster>();
        dft = Time.fixedDeltaTime;
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //move this to new class?
        bool upKeyPressed = moveInput.y > verticalDeadzone;
        bool jumpKeyPressed = Input.GetButtonDown("Jump");
        bool jumpPressed = jumpKeyPressed || upKeyPressed;

        if (Mathf.Abs(moveInput.x) > horizontalDeadzone)
        {
            frameInfo.walkDir = Mathf.Sign(moveInput.x) > 0 ? 1 : -1;
        }
        else
        {
            frameInfo.walkDir = 0;
        }

        if (jumpPressed)
        {
            frameInfo.jump = true;
        }
    }

    private void FixedUpdate()
    {
        earlyJumpCountdownTimer = Mathf.Clamp(earlyJumpCountdownTimer - dft, -1f, float.MaxValue);
        lateJumpCountdownTimer = Mathf.Clamp(lateJumpCountdownTimer - dft, -1f, float.MaxValue); //move this to update?

        Vector2 velocity = rb2d.velocity;
        UnwrapFrameInfo(ref velocity);
        frameInfo = new FrameInfo();

        Raycaster.CollisionInfo collisions = raycaster.Collide(groundCheckProtrusion); //move this to update?
        bool prevIsGrounded = isGrounded;
        isGrounded = collisions.below;
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (prevIsGrounded && !isGrounded)
        {
            lateJumpCountdownTimer = lateJumpTimeTolerance;
        }

        rb2d.velocity = velocity;
    }

    private void UnwrapFrameInfo(ref Vector2 velocity)
    {
        Vector2 v = velocity;
        v.x = frameInfo.walkDir * walkVelocity;

        if (frameInfo.jump)
        {
            if (jumpCount < 1 && !isGrounded && lateJumpCountdownTimer >= 0f)
            {
                DoJump();
            }
            else
            {
                earlyJumpCountdownTimer = earlyJumpTimeTolerance; //move this to update?
            }
        }

        if (jumpCount < 1 && isGrounded && earlyJumpCountdownTimer >= 0f)
        {
            DoJump();
        }

        velocity = v;

        return;

        void DoJump()
        {
            jumpCount++;
            v.y = jumpVelocity;
            earlyJumpCountdownTimer = -1f;
            lateJumpCountdownTimer = -1f;
        }
    }

    private struct FrameInfo
    {
        public int walkDir;
        public bool jump;
    }
}