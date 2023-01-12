using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] monsters;
    public ParticleSystem spawnEffect;
    int randomSpawnPoint, randomMonster;
    public static bool spawnAllowed;
    void Start()
    {
        spawnAllowed = true;
        InvokeRepeating("SpawnAMonster", 0.5f, 2.5f);
    }

    void Update(){
        if(spawnAllowed == false){
            CancelInvoke("SpawnAMonster");
        }
    }

    void SpawnAMonster()
    {
        if (spawnAllowed)
        {
            spawnEffect.Play();
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            randomMonster = Random.Range(0, monsters.Length);
            Instantiate(monsters[randomMonster], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            Instantiate(spawnEffect, spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            AudioManager.instance.PlaySFX("Spawn", 0.2f);
        }
    }
}
