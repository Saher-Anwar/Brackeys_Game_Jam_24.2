using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IEnemy
{
    [Header("Movement Settings")]
    [SerializeField] Player player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 1f;

    [Header("Attack Settings")]
    [SerializeField] float damage = 10f;

    [Header("Test Settings")]
    [SerializeField] float minAttackDistance = 3f;

    [Header("Death Settings")]
    [SerializeField] float deathDelay = 1f;

    float knockbackCooldown = .5f;
    bool isKnockedback = false;

    float attackCooldown = 1f;
    bool isAttacking = false;

    private void Awake() 
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(player == null) return;
        
        if(!isAttacking && Vector2.Distance(transform.position, player.transform.position) < minAttackDistance)
        {
            StartCoroutine(Attack(damage));
            return;
        } 
    }

    private void FixedUpdate() 
    {
        if(isKnockedback || isAttacking) return;

        Move();
    }

    public IEnumerator Attack(float damage)
    {
        if(player == null) yield break;

        isAttacking = true;
        rb.velocity = Vector2.zero;
        player.TakeDamage(damage);

        // TODO: Play animation, VFX, SFX, etc.
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
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

    private void FollowPlayer()
    {
        if (player == null) return;  

        // Set velocity in direction of player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void OnDrawGizmosSelected()
    {
        // Optionally draw a line or arrow showing the direction to the player in the Scene view
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minAttackDistance);
    }
}
