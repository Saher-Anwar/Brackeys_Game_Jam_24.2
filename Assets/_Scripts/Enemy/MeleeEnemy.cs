using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Test Settings")]
    [SerializeField] float minAttackDistance = 3f;

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

    public override IEnumerator Attack(float damage)
    {
        if(player == null) yield break;

        isAttacking = true;
        rb.velocity = Vector2.zero;
        player.TakeDamage(damage);

        // TODO: Play animation, VFX, SFX, etc.
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minAttackDistance);
    }
}