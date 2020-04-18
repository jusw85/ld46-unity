using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb2d.velocity = new Vector2(walkSpeed, 0);
    }

    public void Hurt()
    {
        Destroy(gameObject);
    }

    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }
}