using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
    // vitesse de déplacement
    public float speed = 100f; // normalement 5;

    [Header("ANIMATION")]
    public float spriteChangeDelay = 0.1f;

    [Header("Sprites Default")]
    public Sprite[] spritesNormalWalkHorizontal;

    private Sprite[] currentSprites;
    private int _spriteIndex = 0;
    private float _spriteLastChange = 0f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D col;

    private Vector2 lastDirection;
    private Vector3 originalScale; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0;
        lastDirection = Vector2.zero;

        originalScale = transform.localScale;
    }

    void Update()
    {

        _spriteLastChange += Time.deltaTime;
        if (rb.velocity.magnitude > 0.2f)
        {
            if (_spriteLastChange >= spriteChangeDelay)
            {
                _spriteLastChange = 0f;
                _spriteIndex = (_spriteIndex + 1) % currentSprites.Length;
                sprite.sprite = currentSprites[_spriteIndex];
            }
        }
    }


    private void FixedUpdate()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveInputX * speed, moveInputY * speed);

        if (moveInputX != 0)
        {
            currentSprites = spritesNormalWalkHorizontal;
            sprite.flipX = moveInputX < 0;
        }
        else if (moveInputY > 0)
        {
            currentSprites = spritesNormalWalkHorizontal;
            sprite.flipX = false;
        }
        else if (moveInputY < 0)
        {
            currentSprites = spritesNormalWalkHorizontal;
            sprite.flipX = false;
        }
    }
}