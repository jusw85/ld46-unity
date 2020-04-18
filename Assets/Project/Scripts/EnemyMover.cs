using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.left * walkSpeed;
    }

    public void Hurt()
    {
        Destroy(gameObject);
    }
}