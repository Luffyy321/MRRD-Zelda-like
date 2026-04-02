using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject m_teleportTo = null;
    public DialogManager dialogManager;
    public List<DialogPage> noKeyPages; // Message si pas de clé

    private GameObject m_player = null;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerBehavior inventory = collision.GetComponent<PlayerBehavior>();

            if (inventory != null && inventory.hasKey)
            {
                TeleportPlayer();
                inventory.hasKey = false;
            }
            else
            {
                // Affiche le message si pas de clé
                if (dialogManager != null && noKeyPages.Count > 0)
                    dialogManager.SetDialog(noKeyPages);
            }
        }
    }

    private void TeleportPlayer()
    {
        if (m_teleportTo != null)
        {
            if (this.transform.parent != null)
            {
                this.transform.parent.gameObject.SetActive(false);
                m_teleportTo.transform.parent.gameObject.SetActive(true);
                m_player.transform.position = m_teleportTo.transform.position;
            }
        }
    }
}