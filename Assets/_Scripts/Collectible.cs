using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    [Header("Collectible Settings")]
    [SerializeField] private float healthIncreaseAmount = 20f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject pickupEffect;

    private void Update() {
        // Constant rotation
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Check if the object colliding is the player
        if (collision.gameObject.CompareTag("Player")) {
            if (healthBar != null) {
                // Increase the player's health
                healthBar.addHealth(healthIncreaseAmount);
            }
            // Destroy the collectible after it has been collected
            if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
