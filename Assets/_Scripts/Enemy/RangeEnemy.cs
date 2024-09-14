using System.Collections;
using UnityEngine;

public class RangeEnemy : Enemy
{
    [Header("Attack Settings")]
    [SerializeField] float takeAimTime = 1f;
    [SerializeField] float maxAttackDistance = 10f;
    [SerializeField] float runAwayDistance = 5f;

    [Header("Bullet Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletRotationOffset = 90f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] int bulletLayer;

    public override IEnumerator Attack(float damage)
    {
        // take aim 
        isAttacking = true;
        yield return new WaitForSeconds(takeAimTime);
        Shoot();
    
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void FixedUpdate() {
        if(isAttacking | isKnockedback) return;

        if(Vector2.Distance(transform.position, player.transform.position) <= runAwayDistance){
            RunAway();
            return; 
        }

        if(!isAttacking && Vector2.Distance(transform.position, player.transform.position) < maxAttackDistance){
            rb.velocity = Vector2.zero;
            StartCoroutine(Attack(damage));
            return;
        }

        Move();
    }

    void RunAway(){
        if (player == null) return;  

        // Set velocity in direction of player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = -direction * speed;
    }

    void Shoot(){
        if(player == null) return;
        
        // Calculate direction to mouse and angle at which to shoot bullet then instantiate bullet
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle + bulletRotationOffset));
        bullet.layer = bulletLayer;

        // Add velocity to the bullet in the direction of the mouse
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxAttackDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);
    }
}
