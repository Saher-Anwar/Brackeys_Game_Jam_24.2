using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    [Header("Enemy Details")]
    [SerializeField] protected float health = 100f;

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
    protected float attackCooldown = 1f;
    protected bool isAttacking = false;

    protected virtual void Awake() 
    {
        player = GameManager.Instance.player.GetComponent<Player>();
        if(player == null) Debug.LogError("Player is null");
        rb = GetComponent<Rigidbody2D>();

        GameManager.OnBeforeStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if(state == GameState.Lose){
            this.enabled = false;
        }
    }

    public virtual void ApplyKnockback(Vector2 force, ForceMode2D forceMode = ForceMode2D.Force)
    {
        isKnockedback = true;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    public abstract IEnumerator Attack(float damage);

    public virtual void Die()
    {
        SpawnManager.Instance.panicBar.decreaseFillAmount();
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
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
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
