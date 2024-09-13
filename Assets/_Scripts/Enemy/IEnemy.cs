using System.Collections;
using UnityEngine;

/// <summary>
/// Interface specifies what all an enemy is capable of doing.
/// </summary>
public interface IEnemy
{
    IEnumerator Attack(float damage);
    public void TakeDamage(float damage);
    public void Move();
    public void ApplyKnockback(Vector2 force, ForceMode2D forceMode = ForceMode2D.Force);
    public void Die();
}
