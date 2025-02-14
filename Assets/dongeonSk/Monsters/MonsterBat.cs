using System.Collections;
using System.Collections.Generic;
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
    public int health = 50;
    private int maxHealth;
    private Color originalColor; // Couleur d'origine du monstre

    private float solidDamageTimer = 0f;
    private float passThroughDamageTimer = 0f;

    [Header("Déplacement")]
    public float detectionRadius = 5f;
    public float speed = 3f;

    private Transform target;
    private bool isChasing = false;
    private bool isWaiting = false;

    private void Start()
    {
        maxHealth = health; // Stocke la vie maximale

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Stocke la couleur d'origine

        if (frames.Length > 0)
        {
            spriteRenderer.sprite = frames[0];
        }
    }

    private void Update()
    {
        // Animation du monstre (léger flottement vertical)
        transform.position += new Vector3(0, Mathf.Sin(Time.time * 10) * 0.0001f, 0);

        // Gestion des animations par frame
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


        // Mise à jour de la couleur en fonction des PV
        float healthPercentage = (float)health / maxHealth;
        spriteRenderer.color = Color.Lerp(Color.red, originalColor, healthPercentage);


        if (isChasing && target != null && !isWaiting)
        {
            MoveTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        if (!isWaiting) 
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Solid"))
            {
                target = collider.transform;
                isChasing = true;
                return;
            }
        }

        isChasing = false;
        target = null;
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            Debug.Log("Le monstre a touché le joueur, il s'arrête 2 secondes.");
            StartCoroutine(WaitAfterAttack());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            solidDamageTimer += Time.deltaTime;
            if (solidDamageTimer >= 0.2f)
            {
                GameManagerSk.Instance.UpdateHealth(-10);
                Debug.Log("Le joueur SOLIDE perd 10 PV !");
                solidDamageTimer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            solidDamageTimer = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Solid"))
        {
            solidDamageTimer += Time.deltaTime;
            if (solidDamageTimer >= 0.2f)
            {
                GameManagerSk.Instance.UpdateHealth(-10);
                Debug.Log("Le joueur SOLIDE perd 10 PV !");
                solidDamageTimer = 0f;
            }
        }
        else if (collider.CompareTag("PassThrough"))
        {
            passThroughDamageTimer += Time.deltaTime;
            if (passThroughDamageTimer >= 0.3f)
            {
                health -= 20;
                Debug.Log("Le monstre perd 20 PV toutes les 0.3s en contact avec un joueur PASS THROUGH.");

                if (health <= 0)
                {
                    Destroy(gameObject);
                    GameManagerSk.Instance.UpdateHealthSaveAdd(15);
                    Debug.Log("Le monstre est détruit !");
                }

                passThroughDamageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Solid"))
        {
            solidDamageTimer = 0f;
        }
        else if (collider.CompareTag("PassThrough"))
        {
            passThroughDamageTimer = 0f;
        }
    }

    private IEnumerator WaitAfterAttack()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2f);
        isWaiting = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
