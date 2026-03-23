using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 8;
    public int currentHealth;

    public Image healthBarImage;
    public Sprite[] healthSprites;


    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
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

        UpdateHealthBar();
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSprites.Length > currentHealth)
        {
            healthBarImage.sprite = healthSprites[currentHealth];
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<PlayerBehavior>().enabled = false;
    }
}