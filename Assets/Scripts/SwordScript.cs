using UnityEngine;

public class Sword : MonoBehaviour
{
    private bool canBePickedUp = false;

    void Start()
    {
        // Attend 0.5s avant de pouvoir être ramassée
        Invoke(nameof(EnablePickup), 0.5f);
    }

    public void EnablePickup()
    {
        canBePickedUp = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBePickedUp && collision.CompareTag("Player"))
        {
            PlayerTransform playerTransform = collision.GetComponent<PlayerTransform>();

            if (playerTransform != null && playerTransform.isTransformed)
                return;


            collision.GetComponent<PlayerSword>().ActiveSwordLayer();
            Destroy(gameObject);
        }
    }
}
