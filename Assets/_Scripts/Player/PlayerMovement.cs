using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] TrailRenderer tr;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] float maxSpeed = 7f;

    [Header("Dash Settings")]
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = .2f;
    [SerializeField] float dashingCooldown = 1f;

    [Header("Explosive Shield Settings")]
    [SerializeField] bool canShield = true;
    [SerializeField] bool isShielding;
    [SerializeField] float shieldDeplyDelay = 3f;
    [SerializeField] float shieldDuration = 1f;
    [SerializeField] float shieldCooldown = 20f;
    [SerializeField]float explosionRadius = 5f;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] LayerMask enemyLayer;

    [Header("UI Elements")]
    [SerializeField] Image dashIcon;
    [SerializeField] Text cooldownText;
    [SerializeField] Image shieldIcon;
    [SerializeField] Text shieldCooldownText;

    [Header("Private Variables")]
    private Vector2 currentVelocity;
    private Vector2 input;
    private float nextDashTime = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (isDashing || isShielding) return;

        // Gather input (WASD or Arrow Keys)
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();  // Keep diagonal movement consistent in speed
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){
            StartCoroutine(Dash());
        }
        UpdateCooldownUI();

        if(Input.GetKeyDown(KeyCode.LeftControl) && canShield){
            StartCoroutine(Shield());
        }
    }

    void FixedUpdate()
    {
        if (isDashing || isShielding) return;
        
        // Smooth acceleration and deceleration
        if (input.magnitude > 0) {
            // Accelerate the player towards the target velocity
            currentVelocity = Vector2.MoveTowards(currentVelocity, input * moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else {
            // Decelerate the player to zero when no input
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
        // Clamp the velocity to max speed
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
        // Set the Rigidbody velocity to the calculated smooth velocity
        rb.velocity = currentVelocity;
    }

    IEnumerator Dash() {
        // Start the dash
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(input.x * dashingPower, input.y * dashingPower);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        // End the dash
        tr.emitting = false;
        isDashing = false;
        // Set next dash time
        nextDashTime = Time.time + dashingCooldown;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // Function to update the UI elements for the dash cooldown
    void UpdateCooldownUI() {
        if (dashIcon != null) {
            float remainingTime = Mathf.Max(0, nextDashTime - Time.time);
            cooldownText.text = remainingTime > 0 ? remainingTime.ToString("F1") + "s" : "";
        }
        if (cooldownText != null) {
            float cooldownProgress = Mathf.Clamp01((nextDashTime - Time.time) / dashingCooldown);
            dashIcon.fillAmount = 1-cooldownProgress;
        }
    }

    IEnumerator Shield()
    {
        canShield = false;
        isShielding = true;
        rb.velocity = Vector2.zero;
        // TODO: Activate shield VFX
        yield return new WaitForSeconds(shieldDeplyDelay);
        // TODO: Shield Explosion VFX
        Explode();
        yield return new WaitForSeconds(shieldDuration);
        isShielding = false;
        yield return new WaitForSeconds(shieldCooldown);
        canShield = true;
    }

    private void Explode(){
        // Detect enemies within the explosion radius
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in enemiesInRange)
        {
            // Calculate the direction from the player to the enemy
            Vector2 direction = enemy.transform.position - transform.position;
            direction.Normalize();  // Ensure the direction vector is normalized (length of 1)

            // Apply force to the enemy to push them away
            Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }

        // Optionally: Add explosion effects, sound, etc.
        Debug.Log("Explosion triggered!");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
