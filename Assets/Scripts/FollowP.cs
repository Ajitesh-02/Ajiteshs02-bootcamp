using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.8f;
    [SerializeField] private float damagePerSecond = 10f;

    private Transform player;
    private Rigidbody2D rb;
    private PHealth playerHealth;
    private bool touchingPlayer = false;

    // New: attack state bool (public for Animator or inspector debugging)
    [SerializeField] private bool isAttack = false;

    // Optional: reference to Animator
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Face the player
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        float distance = Vector2.Distance(player.position, transform.position);

        // If close enough, stop and attack; otherwise move
        if (distance > stopDistance && !touchingPlayer)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            rb.velocity = Vector2.zero;
            SetAttackState(false);
        }
        else
        {
            rb.velocity = Vector2.zero;
            SetAttackState(true);
        }

        // Deal damage if touching player
        if (touchingPlayer && playerHealth != null && isAttack)
        {
            playerHealth.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            playerHealth = collision.gameObject.GetComponent<PHealth>();
            SetAttackState(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            SetAttackState(false);
        }
    }

    /// <summary>
    /// Central method to update attack state and optionally trigger Animator
    /// </summary>
    private void SetAttackState(bool state)
    {
        if (isAttack == state) return; // skip redundant updates

        isAttack = state;

        if (animator != null)
        {
            animator.SetBool("isAttack", isAttack);
        }
    }
}
