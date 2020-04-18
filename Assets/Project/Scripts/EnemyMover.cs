using k;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private int maxHp = 10;
    [SerializeField] private float knockBackForce = 2f;
    [SerializeField] private float knockBackRecoverTime = 0.5f;

    private Rigidbody2D rb2d;
    private int hp;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hp = maxHp;
    }

    private void Start()
    {
        rb2d.velocity = new Vector2(walkSpeed, 0);
    }

    private float dampVelocity;

    private void FixedUpdate()
    {
        Vector2 v = rb2d.velocity;
        v.x = Mathf.SmoothDamp(v.x, walkSpeed, ref dampVelocity, knockBackRecoverTime);
        rb2d.velocity = v;
    }

    public void Hurt()
    {
        if (--hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            bool isFacingRight =
                GameObject.FindWithTag(Tags.PLAYER)?.GetComponent<PlayerInput>()?.IsFacingRight ?? true;
            Vector2 force = (isFacingRight ? Vector2.right : Vector2.left) * knockBackForce;
            rb2d.AddForce(force, ForceMode2D.Impulse);
        }
    }

    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }
}