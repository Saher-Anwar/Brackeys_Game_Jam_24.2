using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    [Header("Collectible Settings")]
    [SerializeField] private float healthIncreaseAmount = 20f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject pickupEffect;

    [Header("Popup Settings")]
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Color healthPopupColor = Color.green;


    public static event Action OnCollectibleCollected;
    
    private void Start() {
        healthBar = FindObjectOfType<HealthBar>();
    }

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
                Debug.Log("Player health increased by " + healthIncreaseAmount);
            }
            // Spawn a damage popup at the player's position
            if (damagePopupPrefab != null) {
                Vector3 popupPosition = collision.transform.position + new Vector3(-2f, 0, 0);
                DamagePopup.Create(popupPosition, damagePopupPrefab, healthIncreaseAmount, healthPopupColor);
            }
            // Destroy the collectible after it has been collected
            if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            OnCollectibleCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
