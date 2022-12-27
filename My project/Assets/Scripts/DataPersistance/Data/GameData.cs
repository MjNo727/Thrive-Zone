using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public float health;
    public Vector2 playerPostion;
    public GameData()
    {
        this.health = 100f;
        playerPostion = Vector2.zero;
    }
}
