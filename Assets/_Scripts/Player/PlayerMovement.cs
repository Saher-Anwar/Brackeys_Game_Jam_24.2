using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]    
    private float moveSpeed = 5f;         // Base movement speed
    [SerializeField]
    private float acceleration = 10f;     // How fast the player accelerates to full speed
    [SerializeField]
    private float deceleration = 10f;     // How fast the player slows down when not pressing any keys
    [SerializeField]
    private float maxSpeed = 7f;          // Maximum speed the player can reach

    private Vector2 currentVelocity;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Gather input (WASD or Arrow Keys)
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();  // Keep diagonal movement consistent in speed
    }

    void FixedUpdate()
    {
        // Smooth acceleration and deceleration
        if (input.magnitude > 0)
        {
            // Accelerate the player towards the target velocity
            currentVelocity = Vector2.MoveTowards(currentVelocity, input * moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate the player to zero when no input
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Clamp the velocity to max speed
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);

        // Set the Rigidbody velocity to the calculated smooth velocity
        rb.velocity = currentVelocity;
    }

}
