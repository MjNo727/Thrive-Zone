using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public float timeValue;
    public TextMeshProUGUI timerText;
    private bool endTimeBuzzer = false;
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
            if (Mathf.RoundToInt(timeValue) == 10)
            {
                if (endTimeBuzzer == false)
                {
                    AudioManager.instance.PlaySFX("Buzzer");
                    endTimeBuzzer = true;
                }
            }
            // Debug.Log(timeValue.ToString("F1"));

        }
        else
        {
            timeValue = 0;
        }


        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
