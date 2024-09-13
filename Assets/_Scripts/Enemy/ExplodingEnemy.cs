using System.Collections;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    [SerializeField] float minAttackDistance = 1f;

    private void Awake() 
    {
        if(player == null) Debug.LogError("Player is null");
        rb = GetComponent<Rigidbody2D>();
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

    public override IEnumerator Attack(float damage)
    {
        if(player == null) yield break;

        isAttacking = true;
        rb.velocity = Vector2.zero;
        player.TakeDamage(damage);

        // TODO: Play animation, VFX, SFX, etc.
        yield return new WaitForSeconds(attackCooldown);
        Die();
    }

    protected override void OnDrawGizmosSelected()
    {       
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minAttackDistance);
    }
}
