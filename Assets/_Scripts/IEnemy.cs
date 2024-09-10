using UnityEngine;

/// <summary>
/// Interface specifies what all an enemy is capable of doing.
/// </summary>
public interface IEnemy
{
    public void Attack(float damage);
    public void TakeDamage(float damage);
    public void Move();
    public void Die();
}
