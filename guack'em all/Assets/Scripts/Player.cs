using UnityEngine;

public class Player : MonoBehaviour
{
   
    public float moveSpeed = 5f;

    private Rigidbody2D myRigidbody;
    private Vector2 moveDirection = Vector2.zero;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
  void Update()
{
    moveDirection = Vector2.zero;

    if (Input.GetKey(KeyCode.A)) moveDirection = Vector2.left;
    if (Input.GetKey(KeyCode.D)) moveDirection = Vector2.right;
    if (Input.GetKey(KeyCode.W)) moveDirection = Vector2.up;
    if (Input.GetKey(KeyCode.S)) moveDirection = Vector2.down;

    FlipSprite();
}
 void FixedUpdate()
{
    myRigidbody.linearVelocity = moveDirection * moveSpeed;
}
    void FlipSprite()
    {
        if (moveDirection == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face Left
        }
        else if (moveDirection == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face Right
        }
        else if (moveDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); // Face Up
        }
        else if (moveDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); // Face Down
        }
    }
}
