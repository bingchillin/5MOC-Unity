using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        if (rb == null) return;

        direction = dir.normalized;
        rb.velocity = direction * speed; // Appliquer la direction immédiatement

        // Détruire l'objet après 3 secondes
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return; // Ignore le joueur

        Destroy(gameObject); // Détruit le projectile au contact d'un mur ou d'un ennemi
    }
}