using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D spikesCollider;
    public int damage = 1;
    public float retractDelay = 2f;

    public float activateDuration = 0.5f; // Durée de l'animation "sortie" (anim 2)
    public float retractDuration = 0.5f;  // Durée de l'animation "rentrée" (anim 4)

    private bool isActivated = false;

    public float damageCooldown = 1f; // 1 seconde entre chaque dégât
    private float damageTimer = 0f;

    void Start()
    {
        spikesCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(ActivateSpikes());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spikesCollider.enabled)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                damageTimer = damageCooldown;
            }
        }
    }

    private IEnumerator ActivateSpikes()
    {
        animator.SetTrigger("Activate");
        yield return new WaitForSeconds(activateDuration);

        spikesCollider.enabled = true;

        yield return new WaitForSeconds(retractDelay);

        spikesCollider.enabled = false;
        damageTimer = 0f;

        animator.SetTrigger("Retract");
        yield return new WaitForSeconds(retractDuration);

        isActivated = false;
    }
}