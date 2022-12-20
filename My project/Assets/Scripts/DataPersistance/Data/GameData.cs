using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public int health;
    public Vector2 playerPostion;
    public SerializableDictionary<string, bool> healthCollected;
    public SerializableDictionary<string, bool> metalBoxDestroyed;
    public SerializableDictionary<string, bool> robotsFixed;

    public GameData()
    {
        this.health = 5;
        playerPostion = Vector2.zero;
        healthCollected = new SerializableDictionary<string, bool>();
        metalBoxDestroyed = new SerializableDictionary<string, bool>();
        robotsFixed = new SerializableDictionary<string, bool>();
    }
}
