using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [Header("Animation")]
    public Sprite[] framesUp;
    public Sprite[] framesDown;
    public Sprite[] framesLeft;
    public Sprite[] framesRight;
    public float frameRate = 0.1f;
    public bool loop = true;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;
    private Vector2 lastDirection;

    [Header("Vie du Monstre")]
    public int health = 50;
    private int maxHealth;
    private Color originalColor;

    private float solidDamageTimer = 0f;
    private float passThroughDamageTimer = 0f;

    [Header("Déplacement")]
    public float detectionRadius = 5f;
    public float speed = 3f;
    public float randomMoveInterval = 3f;
    private float randomMoveTimer;

    private Transform target;
    private bool isChasing = false;
    private bool isFleeing = false;
    private bool isWaiting = false;

    // Nouvelle variable : Le boss reste immobile tant qu'il n'a pas détecté le joueur une première fois
    private bool hasActivated = false;

    private void Start()
    {
        health = 1048;
        maxHealth = health;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        randomMoveTimer = randomMoveInterval;
    }

    private void Update()
    {
        // Si le boss n'a pas encore été activé, il reste immobile
        if (!hasActivated) return;

        // Animation du monstre
        if (framesDown.Length == 0 || framesUp.Length == 0 || framesLeft.Length == 0 || framesRight.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (isChasing || isFleeing)
            {
                if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
                {
                    if (lastDirection.x > 0)
                        Animate(framesRight);
                    else
                        Animate(framesLeft);
                }
                else
                {
                    if (lastDirection.y > 0)
                        Animate(framesUp);
                    else
                        Animate(framesDown);
                }
            }
            else
            {
                Animate(framesDown);
            }
        }

        // Mise à jour de la couleur en fonction des PV
        float healthPercentage = (float)health / maxHealth;
        spriteRenderer.color = Color.Lerp(Color.red, originalColor, healthPercentage);

        if (isChasing && target != null && !isWaiting)
        {
            MoveTowardsTarget();
        }
        else if (isFleeing && target != null && !isWaiting)
        {
            MoveAwayFromTarget();
        }
        else
        {
            RandomMovement();
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
                isFleeing = false;

                // Le boss s'active dès le premier contact avec le joueur
                hasActivated = true;
                return;
            }
            else if (collider.CompareTag("PassThrough"))
            {
                target = collider.transform;
                isFleeing = true;
                isChasing = false;

                // Le boss s'active aussi s'il détecte le joueur en PassThrough
                hasActivated = true;
                return;
            }
        }

        isChasing = false;
        isFleeing = false;
        target = null;
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            lastDirection = direction;
        }
    }

    private void MoveAwayFromTarget()
    {
        if (target != null)
        {
            Vector2 direction = (transform.position - target.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, speed * Time.deltaTime);
            lastDirection = direction;
        }
    }

    private void RandomMovement()
    {
        randomMoveTimer -= Time.deltaTime;
        if (randomMoveTimer <= 0)
        {
            randomMoveTimer = randomMoveInterval;
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            lastDirection = randomDirection;
        }

        transform.position += (Vector3)(lastDirection * speed * 0.5f * Time.deltaTime);
    }

    private void Animate(Sprite[] frames)
    {
        currentFrame %= frames.Length;
        spriteRenderer.sprite = frames[currentFrame];
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

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Solid"))
        {
            solidDamageTimer += Time.deltaTime;
            if (solidDamageTimer >= 0.2f)
            {
                GameManagerSk.Instance.UpdateHealth(-10);
                solidDamageTimer = 0f;
            }
        }
        else if (collider.CompareTag("PassThrough"))
        {
            passThroughDamageTimer += Time.deltaTime;
            if (passThroughDamageTimer >= 0.3f)
            {
                health -= 10;
                Debug.Log("Le monstre perd 10 PV en contact avec un joueur PASS THROUGH.");

                if (health <= 0)
                {
                    Destroy(gameObject);
                    GameManagerSk.Instance.UpdateHealthSaveAdd(100);
                    GameManagerSk.Instance.Updatekey(1);
                    Debug.Log("Le boss est détruit, tu as gagné une clé !");
                }

                passThroughDamageTimer = 0f;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
