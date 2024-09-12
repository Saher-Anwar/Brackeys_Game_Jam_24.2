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

    [Header("UI Elements")]
    [SerializeField] Image dashIcon;
    [SerializeField] Text cooldownText;

    [Header("Private Variables")]
    private Vector2 currentVelocity;
    private Vector2 input;
    private float nextDashTime = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void Update() {
        if (isDashing) return;
        // Gather input (WASD or Arrow Keys)
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();  // Keep diagonal movement consistent in speed
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){
            StartCoroutine(Dash());
        }
        UpdateCooldownUI();
    }

    void FixedUpdate() {
        if (isDashing) return;
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

}
