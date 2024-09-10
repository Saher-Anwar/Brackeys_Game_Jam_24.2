using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanicBar : MonoBehaviour {

    [SerializeField] private Image panicBarFill;
    [SerializeField] private float fillRate = 0.1f;

    private float maxPanic = 100;
    private float currentPanic = 0;

    void Start() {
        currentPanic = 0;
        
    }

    void Update() {
        IncreasePanicOverTime();
    }

    public void IncreasePanicOverTime() {
        if (currentPanic < 1f) {
            currentPanic += fillRate * Time.deltaTime;
            currentPanic = Mathf.Clamp(currentPanic, 0f, 1f);
            panicBarFill.fillAmount = currentPanic;
        }
    }

    public void addPanic (float amount) {
        currentPanic += amount;
        currentPanic = Mathf.Clamp(currentPanic, 0, 1);
        panicBarFill.fillAmount = currentPanic;
    }

    public void decreasePanic(float amount) {
        currentPanic -= amount;
        currentPanic = Mathf.Clamp(currentPanic, 0, 1);
        panicBarFill.fillAmount = currentPanic;
    }

}
