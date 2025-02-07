using UnityEngine;
using UnityEngine.InputSystem;

public class IsaacController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movementDirection;
    private Vector2 currentInput;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    private string lastDirection = "Down";

    private Rigidbody2D rb;
    
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireCooldown = 60f; // Temps entre chaque tir
    private float lastFireTime; // Stocke le dernier moment où un tir a été fait

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleAnimations();
        
        if (Input.GetKeyDown(KeyCode.P)) // Quand on appuie sur P
        {
            FireProjectile();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDirection * moveSpeed;
    }

    private void HandleAnimations()
    {
        if (anim == null) return;

        string animationName = (movementDirection == Vector2.zero) ? "Idle" : "Walking";
        anim.Play(animationName + lastDirection);
    }

    private void OnMove(InputValue value)
    {
        currentInput = value.Get<Vector2>().normalized;
        
        // On met à jour `movementDirection` et `lastDirection` en même temps
        if (currentInput != Vector2.zero)
        {
            movementDirection = GetDirection(currentInput);
            UpdateLastDirection(currentInput); // Met à jour la dernière direction
        }
        else
        {
            movementDirection = Vector2.zero;
        }
    }

    private Vector2 GetDirection(Vector2 input)
    {
        return (Mathf.Abs(input.x) > Mathf.Abs(input.y)) ? new Vector2(Mathf.Sign(input.x), 0) : new Vector2(0, Mathf.Sign(input.y));
    }

    private void UpdateLastDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            lastDirection = (input.x > 0) ? "Right" : "Left";
        }
        else
        {
            lastDirection = (input.y > 0) ? "Up" : "Down";
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Assure-toi que tes murs ont bien le tag "Wall"
        {
            Debug.Log("Collision avec un mur !");
        }
    }
    
    private void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Vérifie si le délai entre les tirs est écoulé
        if (Time.time - lastFireTime < fireCooldown) return;

        // Créer le projectile et obtenir son script
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (projScript != null)
        {
            // Envoie le projectile dans la direction actuelle du joueur
            projScript.SetDirection(GetDirectionVector());
        }

        // Met à jour le dernier moment où on a tiré
        lastFireTime = Time.time;
    }


    private Vector2 GetDirectionVector()
    {
        switch (lastDirection)
        {
            case "Up": return Vector2.up;
            case "Down": return Vector2.down;
            case "Left": return Vector2.left;
            case "Right": return Vector2.right;
            default: return Vector2.zero;
        }
    }


}