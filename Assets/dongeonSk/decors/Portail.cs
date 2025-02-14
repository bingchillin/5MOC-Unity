using UnityEngine;
using UnityEngine.SceneManagement; // Nécessaire pour la gestion des scènes

public class Portail : MonoBehaviour
{
    [Header("Scène à charger")]
    public string sceneName;  // Correctement placé

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifier si le nom de la scène est vide
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Le nom de la scène n'est pas spécifié ! Aucun changement de scène effectué.");
            return; // Ne fait rien si la scène n'est pas définie
        }

        // Si un joueur ou un objet Solid touche le portail, charger la scène
        if (other.CompareTag("Player") || other.CompareTag("Solid"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
