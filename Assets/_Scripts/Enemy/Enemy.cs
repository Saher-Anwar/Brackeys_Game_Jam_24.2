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


    public virtual void ApplyKnockback(Vector2 force, ForceMode2D forceMode = ForceMode2D.Force)
    {
    }

    public virtual IEnumerator Attack(float damage)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Die()
    {
    }

    public virtual void Move()
    {
    }

    public virtual void TakeDamage(float damage)
    {
    }
}
