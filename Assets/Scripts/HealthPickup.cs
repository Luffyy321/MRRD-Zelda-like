using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 8;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerHealth>().currentHealth != 8)
        {
            collision.GetComponent<PlayerHealth>().AddHealth(healthAmount);
            Destroy(gameObject);
        }
    }

}
