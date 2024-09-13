using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] private Image healthBarFill;
    [SerializeField] private float fillRate = 10f;

    // Health is in the 0-100 range, Level is in the 1-5 range
    private float maxHealth = 100;
    private float currentHealth = 0;
    private int healthLevel = 5;

    void Start() {
        currentHealth = maxHealth;
        UpdateHealthLevel();
    }

    void Update() {
        // Can remove, just for testing
        // DecreaseHealthOverTime();
    }

    // Funtion to increase health over time
    public void IncreaseHealthOverTime() {
        if (currentHealth < maxHealth) {
            currentHealth += fillRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthBarFill.fillAmount = currentHealth / maxHealth;
            UpdateHealthLevel();
        }
    }

    // Funciton to decrease health over time
    public void DecreaseHealthOverTime() {
        if (currentHealth > 0) {
            currentHealth -= fillRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthBarFill.fillAmount = currentHealth / maxHealth;
            UpdateHealthLevel();
        }
    }

    // Function to add health custom amount
    public void addHealth (float amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBarFill.fillAmount = currentHealth / maxHealth;
        UpdateHealthLevel();
    }

    // Function to decrease health custom amount
    public void decreaseHealth(float amount) {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBarFill.fillAmount = currentHealth / maxHealth;
        UpdateHealthLevel();
    }

    // Function to update health level
    private void UpdateHealthLevel()
    {
        // Determine the health level based on the current health value
        if (currentHealth <= 20) {
            healthLevel = 1;
        }
        else if (currentHealth <= 40) {
            healthLevel = 2;
        }
        else if (currentHealth <= 60) {
            healthLevel = 3;
        }
        else if (currentHealth <= 80) {
            healthLevel = 4;
        }
        else {
            healthLevel = 5;
        }
        // Level can be used to change effects in the game
        // Log the health level here
        Debug.Log("Health Level: " + healthLevel);
    }

    // Function to get current health
    public float GetHealth() {
        return currentHealth;
    }

    // Function to get health level
    public int GetHealthLevel() {
        return healthLevel;
    }

}
