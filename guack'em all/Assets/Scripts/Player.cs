using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D myRigidbody;
    private Vector2 moveDirection = Vector2.zero;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input for both axes to allow diagonal movement
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;

        moveDirection = new Vector2(moveX, moveY).normalized;

        FlipSprite(moveDirection);
        
        // Send speed to Animator
        // animator.SetFloat("Speed", moveDirection.magnitude);
    }

    void FixedUpdate()
    {
        myRigidbody.linearVelocity = moveDirection * moveSpeed;
    }

    void FlipSprite(Vector2 dir)
    {
        // Only flip left/right based on x movement
        if (dir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
        // Do nothing if only moving vertically
    }
}
