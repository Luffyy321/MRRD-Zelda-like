using System.Reflection;
using UnityEngine;

public enum EnemyState
{
    Patrol,
    Chase,
    Attack
}

public class Enemie : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float moveSpeed = 1f;
    public float detectionRange = 3f;
    public float attackRange = 1f;

    public float attackCooldown = 1.5f;

    private float attackTimer;

    private Transform targetPoint;
    private Transform player;

    public float waitTime = 1.5f;

    private float waitCounter;
    private bool isWaiting;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public BoxCollider2D attackCollider;

    public EnemyState state = EnemyState.Patrol;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        targetPoint = pointB;

        attackTimer = attackCooldown; 
        attackCollider.enabled = false;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            state = EnemyState.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            state = EnemyState.Chase;
        }
        else
        {
            state = EnemyState.Patrol;
        }

        switch (state)
        {
            case EnemyState.Patrol:
                Patrouille();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;

            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    void Patrouille()
    {
        if (pointA == null || pointB == null)
            return;

        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = targetPoint.position;

        if (isWaiting)
        {
            waitCounter -= Time.fixedDeltaTime;

            HandleAnimations(Vector2.zero);

            if (waitCounter <= 0f)
            {
                isWaiting = false;
                targetPoint = (targetPoint == pointA) ? pointB : pointA;
            }

            return;
        }

        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        Vector2 direction = targetPosition - currentPosition;
        HandleAnimations(direction);

        if (Vector2.Distance(currentPosition, targetPosition) < 0.05f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    void ChasePlayer()
    {
        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = player.position;

        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        Vector2 direction = targetPosition - currentPosition;
        HandleAnimations(direction);
    }

    void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero;

        Vector2 direction = player.position - transform.position;

        HandleAnimations(direction);

        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer <= 0f)
        {
            if (animator != null)
                animator.SetTrigger("Attack");

            attackTimer = attackCooldown;
        }
    }

    void HandleAnimations(Vector2 direction)
    {
        float speed = direction.magnitude;

        if (animator != null)
            animator.SetFloat("Speed", speed);

        Vector3 scale = transform.localScale;

        if (direction.x < 0f)
            scale.x = Mathf.Abs(scale.x);
        else if (direction.x > 0f)
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }



    public void StartAttack()
    {
        attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        attackCollider.enabled = false;
    }

}

