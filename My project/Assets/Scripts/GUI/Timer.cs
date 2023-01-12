using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour, IDataPersistance
{
    public static Timer instance;
    public float timeValue;
    public TextMeshProUGUI timerText;
    private bool endTimeBuzzer = false;
    public PlayerController player;
    bool teleport = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
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

    public void StartTimer()
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
        }
        else
        {
            timeValue = 0;
            if (teleport == false)
            {
                player.transform.position = new Vector2(37.8f, 6.22f);
                MonsterSpawner.spawnAllowed = false;
                teleport = true;
            }
        }
    }

    public void LoadData(GameData data)
    {
        this.timeValue = data.currentTimeOnTimer;
    }

    public void SaveData(GameData data)
    {
        data.currentTimeOnTimer = this.timeValue;
    }
}
