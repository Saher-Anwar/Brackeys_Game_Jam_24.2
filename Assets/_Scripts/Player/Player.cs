using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private int bulletRotationOffset = 90;

    private void Update() {
        if(Input.GetMouseButtonDown(0)) Shoot();
    }

    public void Shoot(){
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Set z to 0 since we're working in 2D

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calculate the angle of rotation for the bullet
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate the bullet and rotate it
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle + bulletRotationOffset));

        // Add velocity to the bullet in the direction of the mouse
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        Debug.Log("Instantiated bullet and set direction");
    }

    public void TakeDamage(float damage){
        Debug.Log("Player took damage");
    }

    public void Die(){
        Debug.Log("Player died");
        Destroy(gameObject);
    }

    
}
