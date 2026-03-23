using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Enemie>().enabled = false;
        // Détruit l'ennemi après l'animation de mort
        Destroy(gameObject, 1f);
    }
}
