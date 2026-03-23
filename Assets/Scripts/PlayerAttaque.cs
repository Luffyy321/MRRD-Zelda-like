using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D attackCollider;
    private PlayerSword playerSword;
    private PlayerBehavior playerBehavior;

    void Start()
    {
        playerSword = GetComponent<PlayerSword>();
        playerBehavior = GetComponent<PlayerBehavior>();
        attackCollider.enabled = false;
    }

    void Update()
    {
        // Flip la position de la hitbox selon la direction du joueur
        Vector2 offset = attackCollider.offset;
        offset.x = Mathf.Abs(offset.x) * (playerBehavior.IsFacingLeft() ? -1f : 1f);
        attackCollider.offset = offset;


        if (Input.GetMouseButtonDown(0) && playerSword.hasSword)
        {
            animator.SetTrigger("Attack");
        }
    }

    // Appelé par Animation Event au début du coup
    public void StartAttack()
    {
        attackCollider.enabled = true;
    }

    // Appelé par Animation Event à la fin du coup
    public void EndAttack()
    {
        attackCollider.enabled = false;
    }
}