using UnityEngine;
using System.Collections;

public class PlayerSword : MonoBehaviour
{
    public Animator animator;
    private int SwordLayerIndex = 2;
    public bool hasSword = false;
    private bool isThrowing = false;

    public GameObject swordProjectilePrefab;

    public void ActiveSwordLayer()
    {
        hasSword = true;
        
        animator.SetLayerWeight(SwordLayerIndex, 1f);
    }

    public void DeactivateSwordLayer()
    {
        hasSword = false;
        animator.SetLayerWeight(SwordLayerIndex, 0f);
    }

    public void ThrowSword(Vector2 direction)
    {
        if (!hasSword || isThrowing ) return;

        isThrowing = true;
        // 1. Lance l'animation D'ABORD (le layer est encore actif)
        animator.SetTrigger("Throw");

        // 2. Désactive l'épée APRÈS un délai (durée de l'animation de lancer)
        StartCoroutine(DisableSwordAfterThrow(direction));
    }

    private IEnumerator DisableSwordAfterThrow(Vector2 direction)
    {
        // Attend la durée de l'animation de lancer
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(2).length);

        isThrowing = false;
        // Maintenant on désactive le layer et on spawn le projectile
        DeactivateSwordLayer();

        GameObject projectile = Instantiate(swordProjectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<SwordProjectile>().Launch(direction);
    }
}
