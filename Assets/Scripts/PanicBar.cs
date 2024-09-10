using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanicBar : MonoBehaviour {

    [SerializeField] private Image panicBarFill;
    [SerializeField] private float fillRate = 10f;

    // Panic is in the 0-100 range, Level is in the 1-5 range
    private float maxPanic = 100;
    private float currentPanic = 0;
    private int panicLevel = 1;

    void Start() {
        currentPanic = 0;
        UpdatePanicLevel();
    }

    void Update() {
        // Can remove, just for testing
        IncreasePanicOverTime();
    }

    // Funtion to increase panic over time
    public void IncreasePanicOverTime() {
        if (currentPanic < maxPanic) {
            currentPanic += fillRate * Time.deltaTime;
            currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
            panicBarFill.fillAmount = currentPanic / maxPanic;
            UpdatePanicLevel();
        }
    }

    // Function to add panic custom amount
    public void addPanic (float amount) {
        currentPanic += amount;
        currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
        panicBarFill.fillAmount = currentPanic / maxPanic;
        UpdatePanicLevel();
    }

    // Function to decrease panic custom amount
    public void decreasePanic(float amount) {
        currentPanic -= amount;
        currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
        panicBarFill.fillAmount = currentPanic / maxPanic;
        UpdatePanicLevel();
    }

    // Function to update panic level
    private void UpdatePanicLevel()
    {
        // Determine the panic level based on the current panic value
        if (currentPanic <= 20) {
            panicLevel = 1;
        }
        else if (currentPanic <= 40) {
            panicLevel = 2;
        }
        else if (currentPanic <= 60) {
            panicLevel = 3;
        }
        else if (currentPanic <= 80) {
            panicLevel = 4;
        }
        else {
            panicLevel = 5;
        }
        // Level can be used to change the game difficulty
        // Log the panic level here
        Debug.Log("Panic Level: " + panicLevel);
    }

    // Function to get current panic
    public float GetPanic() {
        return currentPanic;
    }

    // Function to get panic level
    public int GetPanicLevel() {
        return panicLevel;
    }

}
