using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Movement Settings")]
    [SerializeField] protected Player player;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float speed = 1f;

    [Header("Attack Settings")]
    [SerializeField] protected float damage = 10f;

    [Header("Death Settings")]
    [SerializeField] protected float deathDelay = 1f;

    protected float knockbackCooldown = .5f;
    protected bool isKnockedback = false;

    public virtual void ApplyKnockback(Vector2 force, ForceMode2D forceMode = ForceMode2D.Force)
    {
        isKnockedback = true;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    public virtual IEnumerator Attack(float damage)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Die()
    {
        Destroy(gameObject, deathDelay);
    }

    public virtual void Move()
    {
        if (player == null) return;  

        // Set velocity in direction of player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public virtual void TakeDamage(float damage)
    {
    }

    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackCooldown);
        isKnockedback = false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        // Optionally draw a line or arrow showing the direction to the player in the Scene view
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}
