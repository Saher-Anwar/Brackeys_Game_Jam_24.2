using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IEnemy
{
    [Header("Movement Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;

    [Header("Death Settings")]
    [SerializeField] private float deathDelay = 1f;

    float knockbackCooldown = .5f;
    bool isKnockedback = false;

    private void Awake() 
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if(isKnockedback) return;

        Move();
    }

    public void Attack(float damage)
    {
    }

    public void Die()
    {
        Destroy(gameObject, deathDelay);
    }

    public void TakeDamage(float damage)
    {
    }

    public void Move() => FollowPlayer();

    public void ApplyKnockback(Vector2 knockbackForce, ForceMode2D forceMode = ForceMode2D.Force)
    {
        isKnockedback = true;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackCooldown);
        isKnockedback = false;
    }

    // Movement function for following the player
    private void FollowPlayer()
    {
        if (player == null) return;  

        // Set velocity in direction of player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    // Draw a line or arrow in the Scene view to visualize the direction to the player
    private void OnDrawGizmosSelected()
    {
        // Optionally draw a line or arrow showing the direction to the player in the Scene view
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
