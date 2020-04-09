using Jusw85.Common;
using UnityEngine;

/// <summary>
/// 
/// ref raycaster handles only rays
/// the reference controller2d extends raycaster, handles slopes
/// 
/// possible layout:
/// raycaster to handle shooting rays + collisions
/// player to handle jumping logic e.g. grounded / not grounded, wallslide, etc.
/// playerinput to read input
/// question: where should handle continuous space down jumping?
/// question: where should handle cant jump when in the middle of being hit?
///
/// possible add scriptable objects as composable behaviours to this e.g. double jump, walk through walls, etc.
/// </summary>
[RequireComponent(typeof(Raycaster), typeof(Rigidbody2D))]
public class PlatformControllerOrig2 : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 4;
    [SerializeField] private float timeToJumpApex = 0.4f;
    [SerializeField] private float walkVelocity = 8;

    [SerializeField] private float jumpPressedTimeTolerance = 0.1f;
    [SerializeField] private float notGroundedJumpTimeTolerance = 0.1f;

    private Raycaster raycaster;
    private Rigidbody2D rb2d;

    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;

    private bool oldIsGrounded;
    private bool isGrounded;
    private float jumpPressedTime = float.MinValue;
    private float notGroundedJumpTime = float.MinValue;

    private Vector2 moveInput;
    private bool isJumping;

    private void Awake()
    {
        raycaster = GetComponent<Raycaster>();
        rb2d = GetComponent<Rigidbody2D>();

        UpdateGravity();
        velocity = Vector3.zero;
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool upKeyPressed = moveInput.y > 0;
        bool jumpKeyPressed = Input.GetButtonDown("Jump");
        bool jumpPressed = jumpKeyPressed || upKeyPressed;

        // Time.time; //affected by timescale
        // float currentTime = Time.deltaTime; //affected by timescale
        // Debug.Log(currentTime);

        // if (Input.GetKey("d"))
        // {
        //     Time.timeScale = 0f;
        // }

        if (jumpPressed)
        {
            // jumpPressedTime = Time.time;
            if (!isJumping && (isGrounded))
            {
                Jump();
            }
        }

        // if ((Time.time - jumpPressedTime) <= jumpPressedTimeTolerance)
        // {
        //     if (!isJumping && (isGrounded || (Time.time - notGroundedJumpTime) <= notGroundedJumpTimeTolerance))
        //     {
        //         Jump();
        //     }
        // }

        velocity.x = walkVelocity * moveInput.x;
        velocity.y += gravity * Time.deltaTime; // gravity is ms-2, also note this is added. Terminal velocity?
        Vector3 displacement = velocity * Time.deltaTime;
        Move(displacement);
    }

    private void OnValidate()
    {
        UpdateGravity();
        jumpPressedTimeTolerance = Mathf.Clamp(jumpPressedTimeTolerance, 0, float.MaxValue);
        notGroundedJumpTimeTolerance = Mathf.Clamp(notGroundedJumpTimeTolerance, 0, float.MaxValue);
    }

    private void Move(Vector3 displacement)
    {
        // Debug.Log(GetComponent<Collider2D>().bounds);
        Raycaster.CollisionInfo collisions = raycaster.Collide(displacement);

        Vector2 p = transform.position;
        p += collisions.vec;

        // transform.Translate(collisions.vec);
        // GetComponent<Rigidbody2D>().MovePosition(p);
        rb2d.position = p;

        if (collisions.below || collisions.above)
        {
            velocity.y = 0;
        }

        oldIsGrounded = isGrounded;
        isGrounded = collisions.below;
        if (isGrounded)
        {
            isJumping = false;
        }

        if (oldIsGrounded && !isGrounded)
        {
            notGroundedJumpTime = Time.time;
        }
    }


    private void Jump()
    {
        velocity.y = jumpVelocity;
        isJumping = true;
        jumpPressedTime = float.MinValue;
        notGroundedJumpTime = float.MinValue;
    }

    private void UpdateGravity()
    {
        gravity = (2 * -jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = -(gravity * timeToJumpApex);
        // Debug.Log(gravity);
    }
}