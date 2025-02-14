using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGelOs : MonoBehaviour
{
    public int points = 10;
    public float pickupRadius = 1.5f; // Distance pour ramasser la gemme

    private void Update()
    {
        // On récupère tous les colliders dans la zone de la gemme
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);

        foreach (var collider in colliders)
        {
            // Vérifie si l'objet en collision a un Rigidbody2D, ce qui signifie qu'il peut être un joueur ou un objet interactif
            if (collider.attachedRigidbody != null)
            {
                // Si l'objet possède un Rigidbody2D, il peut ramasser la gemme
                GameManagerSk.Instance.UpdateBody(points);
                Destroy(gameObject); // Détruire la gemme après avoir été ramassée
                break;  // Sortir de la boucle après avoir ramassé la gemme
            }
        }
    }
}
