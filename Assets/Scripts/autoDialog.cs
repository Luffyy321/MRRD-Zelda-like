using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class autoDialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public List<DialogPage> pages; // Les pages du dialogue à afficher
    private bool hasTriggered = false; // Pour ne déclencher qu'une seule fois

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // Construit la liste de pages et l'envoie au DialogManager
            dialogManager.SetDialog(pages);
        }
    }
}
