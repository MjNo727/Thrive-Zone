using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public float playerCurrentHealth;
    public float playerMaxHealth;
    public float playerCurrentExp;
    public float playerRequiredExp;
    public int playerCurrentLevel;
    public Vector2 playerCurrentPostion;
    public GameData()
    {
        this.playerCurrentHealth = 100f;
        this.playerMaxHealth = 100f;
        this.playerCurrentExp = 0f;
        this.playerRequiredExp = 0f;
        this.playerCurrentLevel = 1;
        playerCurrentPostion = Vector2.zero;
    }
}
