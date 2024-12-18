using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
   
    // vitesse, force de saut
        public float speed = 5f;
        public float jumpForce = 8f;


        [Header("ANIMATION")]
        public Sprite spriteIdle;
        public Sprite spriteJump;
        public float spriteChangeDelay = 0.1f;

        public float _spriteLastChange = 0f;
        private int _spriteIndex = 0;
        public Sprite[] spritesWalk ;



        private Rigidbody2D rb;
        private SpriteRenderer sprite;


    // Update is called once per frame
    void Update()
    {
        _spriteLastChange += Time.deltaTime;

        if (Mathf.Abs(rb.velocity.y) > 0.2f){
             sprite.sprite = spriteJump;


        }else if (Mathf.Abs(rb.velocity.x) > 0.2f){

            if (_spriteLastChange >= spriteChangeDelay){
                _spriteLastChange = 0f;
                _spriteIndex = (_spriteIndex + 1) % spritesWalk.Length;
                sprite.sprite = spritesWalk[_spriteIndex];
            }
        } else {
            sprite.sprite = spriteIdle;
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(speed, 0);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate(){
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(moveInput != 0)
            sprite.flipX = moveInput < 0;

        // Axe vertical > 0 jumpForce
        float jumpInput = Input.GetAxis("Vertical");
        if (jumpInput > 0 && Mathf.Abs(rb.velocity.y) < .001f){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        

    }

    
}
