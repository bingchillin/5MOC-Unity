using UnityEngine;

public class Grass : MonoBehaviour
{
    public int healthGain = 10; // PV gagnés par seconde
    public float interval = 1f; // Temps entre chaque gain de vie
    private float healTimer = 0f; // Timer pour le soin progressif
    private bool playerInZone = false; // Vérifie si le joueur est dans l'herbe

    public AudioClip healSound; // Son de soin
    private AudioSource audioSource;

    private void Start()
    {
        // Ajouter un AudioSource si nécessaire
        if (healSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = healSound;
            audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (playerInZone) // Seulement si le joueur est dans la zone
        {
            healTimer += Time.deltaTime;

            if (healTimer >= interval)
            {
                GameManagerSk.Instance.UpdateHealth(healthGain); // Ajoute de la vie
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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Solid") || other.CompareTag("PassThrough"))
        {
            playerInZone = false;
        }
    }
}
