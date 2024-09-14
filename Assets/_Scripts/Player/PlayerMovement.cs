using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] TrailRenderer trailRenderer;

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

    GameObject canvas;
    private Vector2 currentVelocity;
    private Vector2 input;
    private float nextDashTime = 0f;
    private float nextShieldTime = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start() {
        dashIcon = GameObject.Find("DashImage").GetComponent<Image>();
        cooldownText = GameObject.Find("DashText").GetComponent<Text>();
        shieldIcon = GameObject.Find("ShieldImage").GetComponent<Image>();
        shieldCooldownText = GameObject.Find("ShieldText").GetComponent<Text>();
    }

    void Update() {
        if (isDashing || isShielding) return;

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
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
        rb.velocity = currentVelocity;
    }

    IEnumerator Dash() {

        if (trailRenderer == null) yield break;

        // Start the dash
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(input.x * dashingPower, input.y * dashingPower);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        // End the dash
        trailRenderer.emitting = false;
        isDashing = false;

        // Set next dash time
        nextDashTime = Time.time + dashingCooldown;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // Function to update the UI elements for the dash cooldown
    void UpdateCooldownUI() {
        // Dash cooldown UI update
        if (dashIcon != null && cooldownText != null) {
            float remainingDashTime = Mathf.Max(0, nextDashTime - Time.time);
            cooldownText.text = remainingDashTime > 0 ? remainingDashTime.ToString("F1") + "s" : "";
            float dashCooldownProgress = Mathf.Clamp01((nextDashTime - Time.time) / dashingCooldown);
            dashIcon.fillAmount = 1 - dashCooldownProgress;
        }
        // Shield cooldown UI update
        if (shieldIcon != null && shieldCooldownText != null) {
            float remainingShieldTime = Mathf.Max(0, nextShieldTime - Time.time);
            shieldCooldownText.text = remainingShieldTime > 0 ? remainingShieldTime.ToString("F1") + "s" : "";
            float shieldCooldownProgress = Mathf.Clamp01((nextShieldTime - Time.time) / shieldCooldown);
            shieldIcon.fillAmount = 1 - shieldCooldownProgress;
        }
    }

    IEnumerator Shield() {
        canShield = false;
        isShielding = true;
        rb.velocity = Vector2.zero;
        // TODO: Activate shield VFX
        yield return new WaitForSeconds(shieldDeplyDelay);

        // TODO: Shield Explosion VFX
        Explode();
        yield return new WaitForSeconds(shieldDuration);

        isShielding = false;
        // Set next shield time
        nextShieldTime = Time.time + shieldCooldown;
        yield return new WaitForSeconds(shieldCooldown);

        canShield = true;
    }

    private void Explode() {
        // Detect enemies within the explosion radius
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in enemiesInRange) {
            // Calculate the direction from the player to the enemy
            Vector2 direction = enemy.transform.position - transform.position;
            direction.Normalize();

            // Apply force to the enemy to push them away
            IEnemy currEnemy = enemy.GetComponent<IEnemy>();
            currEnemy.ApplyKnockback(direction * explosionForce, ForceMode2D.Impulse);
        }

        // Todo: Add explosion effects, sound, etc.
        Debug.Log("Explosion triggered!");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
