using System.Collections;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour, IEnemy
{
    public void ApplyKnockback(Vector2 force, ForceMode2D forceMode = ForceMode2D.Force)
    {
    }

    public IEnumerator Attack(float damage)
    {
        yield break;
    }

    public void Die()
    {
    }

    public void Move()
    {
    }

    public void TakeDamage(float damage)
    {
    }
}
