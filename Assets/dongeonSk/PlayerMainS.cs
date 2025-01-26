using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainS : MonoBehaviour
{
    // vitesse de déplacement
    public float speed = 5f;

    [Header("ANIMATION")]
    public Sprite spriteIdle; // (optionnel) utilisé si on veut une image d'arrêt spécifique
    public float spriteChangeDelay = 0.1f;

    private float _spriteLastChange = 0f;
    private int _spriteIndex = 0;

    public Sprite[] spritesWalkHorizontal; // Tableaux pour gauche/droite
    public Sprite[] spritesWalkUp;         // Tableaux pour haut
    public Sprite[] spritesWalkDown;       // Tableaux pour bas

    private Sprite[] currentSprites;       // Tableaux actuellement utilisé pour l'animation

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private Vector2 lastDirection;         // Dernière direction enregistrée

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;

        // Initialisation du tableau par défaut
        currentSprites = spritesWalkHorizontal;
        lastDirection = Vector2.zero; // Pas de mouvement initial
    }

    void Update()
    {
        _spriteLastChange += Time.deltaTime;

        // Gestion de l'animation
        if (rb.velocity.magnitude > 0.2f) // Vérifie si le personnage se déplace
        {
            if (_spriteLastChange >= spriteChangeDelay)
            {
                _spriteLastChange = 0f;
                _spriteIndex = (_spriteIndex + 1) % currentSprites.Length;
                sprite.sprite = currentSprites[_spriteIndex];
            }

            // Met à jour la dernière direction
            lastDirection = rb.velocity.normalized;
        }
        else
        {
            // Si le personnage ne bouge pas, conserver le dernier sprite utilisé
            // Pas besoin de changer le sprite ici
        }
    }

    private void FixedUpdate()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        // Appliquer la vitesse
        rb.velocity = new Vector2(moveInputX * speed, moveInputY * speed);

        // Gérer l'orientation du sprite et changer le tableau d'animation
        if (moveInputX != 0)
        {
            // Déplacement horizontal
            currentSprites = spritesWalkHorizontal;
            sprite.flipX = moveInputX > 0; // Inverser le sprite si on va vers la gauche
        }
        else if (moveInputY > 0)
        {
            // Déplacement vers le haut
            currentSprites = spritesWalkUp;
            sprite.flipX = false; // Pas de flip pour haut/bas
        }
        else if (moveInputY < 0)
        {
            // Déplacement vers le bas
            currentSprites = spritesWalkDown;
            sprite.flipX = false; // Pas de flip pour haut/bas
        }

        // Si le joueur ne donne pas d'input (arrêt total), on conserve le tableau courant et la direction précédente
    }
}
