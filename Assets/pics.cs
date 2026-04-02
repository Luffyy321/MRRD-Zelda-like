using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D spikesCollider;
    public int damage = 1;
    public float retractDelay = 2f; // Temps avant que les pics rentrent

    private bool isActivated = false;

    void Start()
    {
        spikesCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isActivated)
            {
                isActivated = true;
                StartCoroutine(ActivateSpikes(collision));
            }
            else
            {
                // Les pics sont déjà sortis, inflige des dégâts directement
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateSpikes(Collider2D playerCollider)
    {
        // Lance l'animation de sortie
        animator.SetTrigger("Activate");

        // Attend que les pics soient complètement sortis
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Active le collider de dégâts
        spikesCollider.enabled = true;

        // Inflige des dégâts si le joueur est encore dessus
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, spikesCollider.bounds.size, 0f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        // Attend avant de rentrer
        yield return new WaitForSeconds(retractDelay);

        // Lance l'animation de rentrée
        animator.SetTrigger("Retract");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        spikesCollider.enabled = false;
        isActivated = false;
    }
}