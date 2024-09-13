using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [Header("Timer Settings")]
    [SerializeField] public Text timeText;
    public float timeRemaining = 0;
    public bool timerIsRunning = true;
    private Player player;

    void Start() {
        timerIsRunning = true;
        timeText = GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update() {
        // Check if the timer is running and update display
        if (timerIsRunning) {
            if (timeRemaining >= 0) {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
                // Debug.Log("Time Remaining: " + timeRemaining);
            } else {
                Debug.Log("Time has run out!");
            }
        }
        // When the player dies, stop the timer
        if (player == null) {
            timerIsRunning = false;
        }
    }

    void DisplayTime(float timeToDisplay) {

        timeToDisplay += 1;
        // Debug.Log("Time to Display: " + timeToDisplay);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

}
