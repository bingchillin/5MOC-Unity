using UnityEngine;

public class MonsterBat : MonoBehaviour
{
    [Header("Animation")]
    public Sprite[] frames;
    public float frameRate = 0.1f;
    public bool loop = true;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    [Header("Vie du Monstre")]
    public int health = 50; // Points de vie du monstre

    private float damageTimer = 0f; // Timer pour les dégâts récurrents

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (frames.Length > 0)
        {
            spriteRenderer.sprite = frames[0]; 
        }
    }

    private void Update()
    {
        // Légère oscillation pour forcer la détection des collisions même si immobile
        transform.position += new Vector3(0, Mathf.Sin(Time.time * 10) * 0.0001f, 0);

        // Animation du monstre
        if (frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                currentFrame = loop ? 0 : frames.Length - 1;
                return;
            }

            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    // Détecte l'entrée en collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision avec : " + collision.gameObject.name);
    }

    // Détecte si l'objet reste en collision et applique des dégâts toutes les 2 secondes
    private void OnCollisionStay2D(Collision2D collision)
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= 2f) // Applique des dégâts toutes les 2 secondes
        {
            if (collision.gameObject.CompareTag("Solid"))
            {
                GameManagerSk.Instance.UpdateHealth(-10);
                Debug.Log("Le joueur SOLIDE perd 10 PV !");
            }
            else if (collision.gameObject.CompareTag("PassThrough"))
            {
                health -= 10;
                Debug.Log("Le monstre perd 10 PV en restant en contact avec un joueur PASS THROUGH.");

                if (health <= 0)
                {
                    Destroy(gameObject);
                    Debug.Log("Le monstre est détruit !");
                }
            }

            damageTimer = 0f; // Réinitialise le timer après l'application des dégâts
        }
    }

    // Détecte la sortie de collision pour réinitialiser le timer
    private void OnCollisionExit2D(Collision2D collision)
    {
        damageTimer = 0f; // Réinitialise le timer dès qu'on sort de la collision
        Debug.Log("Fin de collision avec : " + collision.gameObject.name);
    }

    // Si la collision utilise un trigger (ex: zone d'effet), applique aussi des dégâts en continu
    private void OnTriggerStay2D(Collider2D collider)
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= 2f) // Applique des dégâts toutes les 2 secondes
        {
            if (collider.CompareTag("Solid"))
            {
                GameManagerSk.Instance.UpdateHealth(-10);
                Debug.Log("Le joueur SOLIDE perd 10 PV !");
            }
            else if (collider.CompareTag("PassThrough"))
            {
                health -= 20;
                Debug.Log("Le monstre perd 20 PV en restant en contact avec un joueur PASS THROUGH.");

                if (health <= 0)
                {
                    Destroy(gameObject);
                    Debug.Log("Le monstre est détruit !");
                }
            }

            damageTimer = 0f; // Réinitialise le timer après l'application des dégâts
        }
    }

    // Réinitialisation du timer à la sortie du trigger
    private void OnTriggerExit2D(Collider2D collider)
    {
        damageTimer = 0f;
        Debug.Log("Fin du trigger avec : " + collider.gameObject.name);
    }
}
