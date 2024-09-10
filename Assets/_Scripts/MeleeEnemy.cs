using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IEnemy
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 1f;

    private float deathDelay = 1f;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate() {
        Move();
    }

    public void Attack(float damage)
    {
    }

    public void Die()
    {
        Destroy(this, deathDelay);
    }

    public void Move() => FollowPlayer();

    public void TakeDamage(float damage)
    {
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
