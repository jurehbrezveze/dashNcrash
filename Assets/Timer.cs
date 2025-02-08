using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private bool timerRunning = true;
    private float timeRemaining = 0f;
    public TMP_Text timerText;

    void Start()
    {
        
    }

    void Update()
    {
        StopWatch();
    }

    void StopWatch()
        {
            if (timerRunning)
            {
                timeRemaining += Time.deltaTime;
                UpdateTimerDisplay();
            }
        }

        void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            int milliseconds = Mathf.FloorToInt((timeRemaining * 100) % 100);

            timerText.text = string.Format("{0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
}
