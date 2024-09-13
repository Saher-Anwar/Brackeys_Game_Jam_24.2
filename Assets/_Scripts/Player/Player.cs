using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] HealthBar healthBar;

    [Header("Bullet Settings")]
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private int bulletRotationOffset = 90;

    private void Update() {
        if(Input.GetMouseButtonDown(0)) Shoot();
    }

    public void Shoot() {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate direction to mouse and angle at which to shoot bullet then instantiate bullet
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle + bulletRotationOffset));

        // Add velocity to the bullet in the direction of the mouse
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

    }

    public void TakeDamage(float damage) {
        if(healthBar.GetHealth() <= 0){
            Die();
            return;
        }
        
        healthBar.decreaseHealth(damage);

        // TODO: Add VFX & graphics    
    }

    public void Die() {
        Debug.Log("Player died");
        Destroy(gameObject);
    }

}
