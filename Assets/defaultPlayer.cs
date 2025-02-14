using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class defaultPlayer : MonoBehaviour
{
    // vitesse de déplacement
    public float speed = 100f; // normalement 5;

    [Header("ANIMATION")]
    public float spriteChangeDelay = 0.1f;

    [Header("Sprites Default")]
    public Sprite[] spritesNormalWalkHorizontal;
    public Sprite[] spritesNormalWalkUp;
    public Sprite[] spritesNormalWalkDown;
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
        currentSprites = spritesNormalWalkHorizontal;
        lastDirection = Vector2.zero;

        originalScale = transform.localScale;


    }

    void Update()
    {


    }


    private void FixedUpdate()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveInputX * speed, moveInputY * speed);

        if (moveInputX != 0)
        {
            sprite.flipX = moveInputX < 0;
        }
        else if (moveInputY > 0)
        {
            if(isDefault)
            {
                currentSprites = spritesNormalWalkUp;
            }
            else if(isZombie)
            {
                currentSprites = spritesZombieWalkUp;
            }
            else if(isIsaac)
            {
                currentSprites = spritesIsaacWalkUp;
            }
            else if(isMegaMan)
            {
                currentSprites = spritesMegaManWalkUp;
            }

            sprite.flipX = false;
        }
        else if (moveInputY < 0)
        {
            if(isDefault)
            {
                currentSprites = spritesNormalWalkDown;
            }
            else if(isZombie)
            {
                currentSprites = spritesZombieWalkDown;
            }
            else if(isIsaac)
            {
                currentSprites = spritesIsaacWalkDown;
            }
            else if(isMegaMan)
            {
                currentSprites = spritesMegaManWalkDown;
            }
            sprite.flipX = false;
        }
    }
}