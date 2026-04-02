using System.Collections;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public int monsterLayerIndex = 1;
    public int swordLayerIndex = 2;
    public GameObject swordPrefab;


    public GameObject transformIndicator;

    public PlayerMana mana;
    public float manaDrainRate = 2f; // Mana drain par seconde pendant transformation

    public float transformRange = 2f; // Distance max pour transformer un ennemi
    public GameObject monsterToDestroy;

    public bool isTransformed = false;
    private GameObject nearbyEnemy;

    void Start()
    {

    }

    void Update()
    {

        UpdateNearbyEnemy();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isTransformed && mana.currentMana >= mana.maxMana && nearbyEnemy != null)
            {
                isTransformed = true;
                monsterToDestroy = nearbyEnemy;
                StartCoroutine(TransformRoutine());
            }
            else if (isTransformed)
            {
                EndTransformation();
            }
        }

        if (isTransformed)
        {
            DrainMana();
        }
    }

    private void UpdateNearbyEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        nearbyEnemy = null;

        float minDistance = transformRange;

        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= minDistance)
            {
                nearbyEnemy = enemy;
                minDistance = distance;
            }
        }

        // ---- INDICATEUR ----
        if (transformIndicator != null)
        {
            // Affiche uniquement si l’ennemi est à portée ET la mana est pleine
            transformIndicator.SetActive(nearbyEnemy != null && mana.currentMana >= mana.maxMana && !isTransformed);

        }
    }

    void DrainMana()
    {
        mana.currentMana -= manaDrainRate * Time.deltaTime;
        if (mana.currentMana <= 0f)
        {
            mana.currentMana = 0f;
            EndTransformation();
        }
        mana.UpdateManaBar();
    }

    public void DestroyEnemy()
    {
        if (monsterToDestroy != null)
        {
            Destroy(monsterToDestroy);
            monsterToDestroy = null;
        }
    }

    public void ActivateMonsterLayer()
    {
        animator.SetLayerWeight(monsterLayerIndex, 1f);
        // Passe sur le layer qui peut traverser l'eau
        gameObject.layer = LayerMask.NameToLayer("PlayerMouche");
    }

    IEnumerator TransformRoutine()
    {
        PlayerSword playerSword = GetComponent<PlayerSword>();

        if (playerSword.hasSword)
        {
            playerSword.DeactivateSwordLayer();
            Instantiate(swordPrefab, transform.position, Quaternion.identity);
        }


        animator.SetTrigger("Transform");

        yield return null;
    }

    void EndTransformation()
    {
        isTransformed = false;
        animator.SetLayerWeight(monsterLayerIndex, 0f);
        // Retour au layer normal bloqué par l'eau
        gameObject.layer = LayerMask.NameToLayer("PlayerNormal");

    }

    // Détection des ennemis proches
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            nearbyEnemy = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (nearbyEnemy == other.gameObject)
                nearbyEnemy = null;
        }
    }

    public void HandleFlip(Vector2 direction)
    {
        if (!isTransformed)
        {
            spriteRenderer.flipX = direction.x < 0f;
        }
        else
        {
            spriteRenderer.flipX = direction.x > 0f;
        }
    }
}