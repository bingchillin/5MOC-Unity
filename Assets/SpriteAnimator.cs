using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] frames; 
    public float frameRate = 2f; 
    public bool loop = true; 

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (frames.Length > 0)
        {
            spriteRenderer.sprite = frames[0]; 
        }
    }

    void Update()
    {
        if (frames.Length == 0) return; 

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                {
                    currentFrame = 0; 
                }
                else
                {
                    currentFrame = frames.Length - 1; 
                    return;
                }
            }

            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}
