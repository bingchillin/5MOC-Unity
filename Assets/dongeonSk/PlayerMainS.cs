using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainS : MonoBehaviour
{
    // vitesse de déplacement
    public float speed = 100f; // normalement 5;

    [Header("ANIMATION")]
    public float spriteChangeDelay = 0.1f;
    private float _spriteLastChange = 0f;
    private int _spriteIndex = 0;

    [Header("Sprites Mode Normal")]
    public Sprite[] spritesNormalWalkHorizontal;
    public Sprite[] spritesNormalWalkUp;
    public Sprite[] spritesNormalWalkDown;

    [Header("Sprites Mode Fantôme")]
    public Sprite[] spritesGhostWalkHorizontal;
    public Sprite[] spritesGhostWalkUp;
    public Sprite[] spritesGhostWalkDown;

    private Sprite[] currentSprites;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D col;
    private bool isSolid = true; 

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

        SetSolidMode(true);
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
            lastDirection = rb.velocity.normalized;
        }

      
        if (Input.GetKeyDown(KeyCode.P))
        {
            isSolid = !isSolid;
            SetSolidMode(isSolid);
        }
    }

    private void FixedUpdate()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveInputX * speed, moveInputY * speed);

        if (moveInputX != 0)
        {
            currentSprites = isSolid ? spritesNormalWalkHorizontal : spritesGhostWalkHorizontal;
            sprite.flipX = moveInputX < 0;
        }
        else if (moveInputY > 0)
        {
            currentSprites = isSolid ? spritesNormalWalkUp : spritesGhostWalkUp;
            sprite.flipX = false;
        }
        else if (moveInputY < 0)
        {
            currentSprites = isSolid ? spritesNormalWalkDown : spritesGhostWalkDown;
            sprite.flipX = false;
        }
    }

  
    void SetSolidMode(bool solid)
    {
        if (solid)
        {
            gameObject.tag = "Solid";
            col.isTrigger = false; 
            transform.localScale = originalScale;
            Debug.Log("Mode SOLIDE activé !");
        }
        else
        {
            gameObject.tag = "PassThrough";
            col.isTrigger = true;
            transform.localScale = originalScale * 0.5f; 
            Debug.Log("Mode TRAVERSABLE activé !");
        }

        currentSprites = solid ? spritesNormalWalkHorizontal : spritesGhostWalkHorizontal;
        _spriteIndex = 0; 
        sprite.sprite = currentSprites[_spriteIndex]; 
    }
}
