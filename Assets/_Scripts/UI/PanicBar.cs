using UnityEngine;
using UnityEngine.UI;
public class PanicBar : MonoBehaviour {

    [SerializeField] Image panicBarFill;
    [SerializeField] float fillAmount = 1f;
    [SerializeField] float fillDelay = 1f;
    [SerializeField] float panicIncreasePerEnemySpawned = 0.1f;
    [SerializeField] float panicDecreasePerEnemyKilled = 0.1f;

    float panicIncreasePerSecond = 0f;

    // Panic is in the 0-100 range, Level is in the 1-5 range
    float maxPanic = 100;
    float currentPanic = 0;
    int panicLevel = 1;

    void Start() {
        currentPanic = 0;
        panicIncreasePerSecond = fillAmount / fillDelay;
        UpdatePanicLevel();
    }

    void Update() {       
        // Can remove, just for testing
        IncreasePanicOverTime();
        UpdatePanicLevel();
    }

    // Funtion to increase panic over time
    public void IncreasePanicOverTime() {
        currentPanic = Mathf.Clamp(currentPanic + (panicIncreasePerSecond * Time.deltaTime), 0, maxPanic);
        panicBarFill.fillAmount = currentPanic / maxPanic;
    }

    public void IncreaseFillAmount() => fillAmount += panicIncreasePerEnemySpawned;
    public void decreaseFillAmount() => fillAmount -= panicDecreasePerEnemyKilled;

    // Function to add panic custom amount
    public void addPanic (float amount) {
        currentPanic += amount;
        currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
        panicBarFill.fillAmount = currentPanic / maxPanic;
    }

    // Function to decrease panic custom amount
    public void decreasePanic(float amount) {
        currentPanic -= amount;
        currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
        panicBarFill.fillAmount = currentPanic / maxPanic;
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
