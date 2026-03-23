/* Author : Raphaël Marczak - 2018/2020, for MIAMI Teaching (IUT Tarbes) and MMI Teaching (IUT Bordeaux Montaigne)
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : MonoBehaviour
{
    Rigidbody2D m_rb2D;

    float m_launchedTime;
    public float m_maxRange = 50f;
    public float m_speed = 10f; // réduit car AddForce Impulse avec 100 c'est très rapide
    Vector2 m_startPosition;

    public GameObject swordPickupPrefab;

    void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_startPosition = transform.position;
    }

    public void Launch(Vector2 direction)
    {
        m_rb2D.AddForce(direction.normalized * m_speed, ForceMode2D.Impulse);

        // Calcule l'angle de la direction et applique la rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        float distanceTravelled = Vector2.Distance(m_startPosition, transform.position);
        if (distanceTravelled >= m_maxRange)
        {
            SpawnSwordPickup();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "SwordProjectile")
        {
            // Dégâts à l'ennemi si touché
            HealthEnemy enemyHealth = collision.gameObject.GetComponent<HealthEnemy>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(1);

            SpawnSwordPickup();
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    private void SpawnSwordPickup()
    {
        // Spawn l'épée ramassable à la position actuelle du projectile
        Instantiate(swordPickupPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}