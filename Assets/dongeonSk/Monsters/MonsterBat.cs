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
    public int health = 100; 

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
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (frames.Length > 0)
        {
            spriteRenderer.sprite = frames[0]; 
        }
    }

    private void Update()
    {
       
        transform.position += new Vector3(0, Mathf.Sin(Time.time * 10) * 0.0001f, 0);

        
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

      
        if (isChasing && target != null && !isWaiting)
        {
            MoveTowardsTarget();
        }
    }

    // Détecte si un joueur est proche et doit être poursuivi
    private void FixedUpdate()
    {
        if (!isWaiting) // Si le monstre pas en pause
        {
            FindTarget();
        }
    }

    // Recherche une cible valide dans le rayon d'activation
    private void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Solid")) // Seul Solid est détecté
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
            if (passThroughDamageTimer >= 0.7f) 
            {
                health -= 20;
                Debug.Log("Le monstre perd 20 PV toutes les 0.7s en contact avec un joueur PASS THROUGH.");

                if (health <= 0)
                {
                    Destroy(gameObject);
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
