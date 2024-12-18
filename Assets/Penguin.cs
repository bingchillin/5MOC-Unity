using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    // vitesse de déplacement
    public float speed = 5f;

    [Header("ANIMATION")]
    public Sprite spriteIdle;
    public Sprite spriteJump;
    public float spriteChangeDelay = 0.1f;

    public float _spriteLastChange = 0f;
    private int _spriteIndex = 0;
    public Sprite[] spritesWalk;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Update()
    {
        _spriteLastChange += Time.deltaTime;

        // Gestion de l'animation
        if (Mathf.Abs(rb.velocity.magnitude) > 0.2f) // Vérifie si le personnage se déplace
        {
            if (_spriteLastChange >= spriteChangeDelay)
            {
                _spriteLastChange = 0f;
                _spriteIndex = (_spriteIndex + 1) % spritesWalk.Length;
                sprite.sprite = spritesWalk[_spriteIndex];
            }
        }
        else
        {
            sprite.sprite = spriteIdle;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        
        rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
      
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        // Appliquer la vitesse
        rb.velocity = new Vector2(moveInputX * speed, moveInputY * speed);

        // Gérer l'orientation du sprite
        if (moveInputX != 0)
            sprite.flipX = moveInputX < 0;
    }
}
