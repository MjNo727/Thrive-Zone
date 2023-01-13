using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public string sceneName;
    public float playerCurrentHealth;
    public float playerMaxHealth;
    public float playerCurrentExp;
    public float playerRequiredExp;
    public int playerCurrentLevel;
    public int playerUpgradePoints;
    public Vector2 playerCurrentPosition;

    public float bossCurrentHealth;

    public float currentTimeOnTimer;

    public GameData()
    {
        this.sceneName = "GameplayScene1";
        this.playerCurrentHealth = 100f;
        this.playerMaxHealth = 100f;
        this.playerCurrentExp = 0f;
        this.playerRequiredExp = 0f;
        this.playerCurrentLevel = 1;
        this.playerUpgradePoints = 0;
        playerCurrentPosition = new Vector2(-20.46f, 2.19f);

        this.bossCurrentHealth = 800f;

        this.currentTimeOnTimer = 1200f;
    }
}
