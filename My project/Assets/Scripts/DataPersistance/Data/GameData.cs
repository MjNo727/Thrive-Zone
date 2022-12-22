using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public int health;
    public Vector2 playerPostion;
    public Vector2 bossPosition;
    public GameData()
    {
        this.health = 10;
        playerPostion = Vector2.zero;
    }
}
