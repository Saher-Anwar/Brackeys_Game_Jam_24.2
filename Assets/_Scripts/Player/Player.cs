using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] float lightFlickerThreshold = 30f; 

    [Header("Bullet Settings")]
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private int bulletRotationOffset = 90;
    [SerializeField] int bulletLayer; // used to mark the bullets created by this player

    [Header("Damage Popup Settings")]
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Color playerPopupColor = Color.red;

    GameObject canvas;
    PanicColorChange panicColorChange;
    private void Start() {
        canvas = GameObject.Find("Canvas");
        healthBar = canvas.GetComponent<HealthBar>();
        panicColorChange = GetComponentInChildren<PanicColorChange>();
    }

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
        bullet.layer = bulletLayer;

        // Add velocity to the bullet in the direction of the mouse
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }

    public void TakeDamage(float damage) {
        healthBar.decreaseHealth(damage);

        // Spawn damage popup at player position
        if (damagePopupPrefab != null) {
            Vector3 popupPosition = transform.position + new Vector3(-2f, 0, 0);
            DamagePopup.Create(popupPosition, damagePopupPrefab, damage, playerPopupColor);
        }

        if (healthBar.GetHealth() <= 0){
            Die();
            return;
        }

        if(healthBar.GetHealth() <= lightFlickerThreshold){
            panicColorChange.InitiateFlickering();
        }

        if(healthBar.GetHealth() > lightFlickerThreshold){
            panicColorChange.StopFlickering();
        }
        
        // TODO: Add VFX & graphics    
    }

    public void Die() {
        GameManager.Instance.ChangeState(GameState.Lose);
        Destroy(gameObject);
    }

}
