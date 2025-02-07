using UnityEngine;
using UnityEngine.InputSystem;

public class IsaacController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movementDirection;
    private Vector2 currentInput;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    private string lastDirection = "Down";

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDirection * moveSpeed;
    }

    private void HandleAnimations()
    {
        if (anim == null) return;

        string animationName = (movementDirection == Vector2.zero) ? "Idle" : "Walking";
        anim.Play(animationName + lastDirection);
    }

    private void OnMove(InputValue value)
    {
        currentInput = value.Get<Vector2>().normalized;
        
        // On met à jour `movementDirection` et `lastDirection` en même temps
        if (currentInput != Vector2.zero)
        {
            movementDirection = GetDirection(currentInput);
            UpdateLastDirection(currentInput); // Met à jour la dernière direction
        }
        else
        {
            movementDirection = Vector2.zero;
        }
    }

    private Vector2 GetDirection(Vector2 input)
    {
        return (Mathf.Abs(input.x) > Mathf.Abs(input.y)) ? new Vector2(Mathf.Sign(input.x), 0) : new Vector2(0, Mathf.Sign(input.y));
    }

    private void UpdateLastDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            lastDirection = (input.x > 0) ? "Right" : "Left";
        }
        else
        {
            lastDirection = (input.y > 0) ? "Up" : "Down";
        }
    }
}