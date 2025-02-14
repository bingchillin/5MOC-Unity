using UnityEngine;

public class WallDead : MonoBehaviour
{
    public int healthLost = 15; // PV perdu par seconde
    public float interval = 1f; // Temps entre chaque perte de vie
    private float healTimer = 0f; // Timer pour la perte progressive
    private bool playerInZone = false; // Vérifie si le joueur est dans l'herbe
    private string currentPlayerTag = ""; // Stocke le tag actuel du joueur

    public AudioClip healSound; // Son de soin
    private AudioSource audioSource;

    private void Start()
    {
        // Configure l'audio
        if (healSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = healSound;
            audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (playerInZone) // Si le joueur est dans la zone
        {
            healTimer += Time.deltaTime;

            // Perte de vie seulement si le joueur est en mode Solid
            if (healTimer >= interval && currentPlayerTag == "Solid")
            {
                GameManagerSk.Instance.UpdateHealthLost(healthLost); 
                healTimer = 0f; // Réinitialise le timer

                // Joue un son de soin si disponible
                if (audioSource != null && healSound != null)
                    audioSource.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Solid") || other.CompareTag("PassThrough"))
        {
            playerInZone = true;
            healTimer = 0f; // Réinitialise le timer quand il entre
            currentPlayerTag = other.tag; // Stocke le tag actuel du joueur
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Mets à jour le tag actuel si le joueur change de forme
        if (other.CompareTag("Solid") || other.CompareTag("PassThrough"))
        {
            currentPlayerTag = other.tag;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Solid") || other.CompareTag("PassThrough"))
        {
            playerInZone = false;
            currentPlayerTag = ""; // Réinitialise le tag
        }
    }
}
