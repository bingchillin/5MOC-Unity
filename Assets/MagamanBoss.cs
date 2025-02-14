using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagamanBoss : MonoBehaviour
{
    public Transform player;        // Référence au joueur
    public float speed = 5f;        // Vitesse de déplacement du monstre
    public int damage = 4;       // Dégâts infligés au joueur
    private bool isParalyzed = false; // Indique si le monstre est paralysé
    private bool isActive = false; // Indique si le monstre est actif

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isActive)
        {
            if (!isParalyzed)
            {
                // Déplacement vers le joueur
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Le monstre a heurté un mur, il se paralyse
            StartCoroutine(Paralyze());
        }

        if (collision.gameObject.CompareTag("Player")){
            GameManagerMegaman.Instance.UpdateHealth(damage*-1);
        }

        
    }

    IEnumerator Paralyze()
    {
        isParalyzed = true;
        yield return new WaitForSeconds(3f);
        isParalyzed = false;
    }

    public void ActivateBoss()
    {
        if (!isActive)
        {
            isActive = true;
            Debug.Log("Le boss est maintenant actif !");
        }
    }
}
