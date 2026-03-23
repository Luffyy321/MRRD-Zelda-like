using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    public int manaAmount = 2;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerMana>().currentMana != 8)
        {
            collision.GetComponent<PlayerMana>().AddMana(manaAmount);
            Destroy(gameObject);
        }
    }
}