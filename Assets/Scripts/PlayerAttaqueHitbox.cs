using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{

    public int addManaAmount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<HealthEnemy>().TakeDamage(1);
            GetComponentInParent<PlayerMana>().AddMana(addManaAmount);
        }
    }
}