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

    // Variables pour le pouvoir
    private bool isPowerActive = false;
    private int powerTime;
    private float timer = 0f; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0;
        currentSprites = spritesNormalWalkHorizontal;
        lastDirection = Vector2.zero;

         originalScale = transform.localScale;

        if (GameManagerSk.Instance != null)
        {
            powerTime = GameManagerSk.Instance.GetPowerTime();
        }
        else
        {
            Debug.LogError("GameManagerSk.Instance est NULL !");
            powerTime = 0;
        }


        SetSolidMode(true);
    }

    void Update()
{
    timer += Time.deltaTime;
    powerTime = GameManagerSk.Instance.GetPowerTime(); 

    if (Input.GetKeyDown(KeyCode.P) && powerTime > 0)
    {
        isSolid = !isSolid;
        SetSolidMode(isSolid);
    }

    _spriteLastChange += Time.deltaTime;

   
    if (powerTime <= 0 && isPowerActive) 
    {
        SetSolidMode(true); 
    }

  
    if (isPowerActive)
    {
        if (timer >= 1f) // Toutes les secondes
        {
            powerTime--;
            GameManagerSk.Instance.UpdatePowerTime(powerTime);

            // Vérification pour arrêter lorsque powerTime atteint 0
            if (powerTime <= 0)
            {
                SetSolidMode(true); 
                powerTime = 0; // Assurer qu'on ne dépasse pas zéro
            }

            timer = 0f;
        }
    }

 
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
            isPowerActive = false;
            isSolid = true;
            GameManagerSk.Instance.UpdateUsePower(false);

           
            currentSprites = spritesNormalWalkHorizontal;
            _spriteIndex = 0;
            sprite.sprite = currentSprites[_spriteIndex]; 

            Debug.Log("Mode SOLIDE activé !");
            transform.localScale = originalScale;
        }
        else
        {
            if (powerTime <= 0) return; 

            gameObject.tag = "PassThrough";
            col.isTrigger = true;
            isPowerActive = true;
            isSolid = false;
            GameManagerSk.Instance.UpdateUsePower(true);

            currentSprites = spritesGhostWalkHorizontal;
            _spriteIndex = 0;
            sprite.sprite = currentSprites[_spriteIndex]; 

            Debug.Log("Mode TRAVERSABLE activé !");
            transform.localScale = originalScale * 0.5f;
        }
    }


}