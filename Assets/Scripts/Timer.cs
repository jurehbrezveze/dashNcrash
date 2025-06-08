using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public bool timerRunning = true;
    public float time = 0f;
    public TMP_Text timerText;
    private AlwaysLoadedScript alwaysLoadedScript;

    void Start()
    {
        GameObject obj = GameObject.Find("AlwaysLoaded");

        if (obj != null)
        {
            alwaysLoadedScript = obj.GetComponent<AlwaysLoadedScript>();
        }
        if(alwaysLoadedScript.sMode == true)
        {
            time = alwaysLoadedScript.time;
        }
    }

    void Update()
    {
        StopWatch();
    }

    void StopWatch()
        {
            if (timerRunning)
            {
                time += Time.deltaTime;
                UpdateTimerDisplay();
            }
            else if(alwaysLoadedScript.sMode == true)
            {
                alwaysLoadedScript.time = time;
                UpdateTimerDisplay();
            }
        }

        void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time * 100) % 100);

            timerText.text = string.Format("{0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
}
