using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] GameObject regularPrefab;
    [SerializeField] GameObject boomerPrefab;

    [SerializeField][Range(0, 100)] int spawnBoomerChanсe; 

    [SerializeField] float spawnRate = 20f;

    [SerializeField] int startMaxHealth = 100;
    [SerializeField] int startDamage = 50;
    [SerializeField] float startLevel = 1f;

    bool canSpawn = false;
    float nextSpawn;

    void Start() {
        LevelGenerator.singleton.OnEndOfLevelGeneration += BeginSpawn;    
    }

    void Update() {
        Spawn();
    }
    
    void Spawn() {
        if (Time.time >= nextSpawn) {
            nextSpawn = Time.time + spawnRate;

            Vector3 pos = transform.position
                    + new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 1.5f), Random.Range(-1f, 1f));

            pos.x = Mathf.Clamp(pos.x, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.z = Mathf.Clamp(pos.z, -GameManager.GameFieldLength, GameManager.GameFieldLength);

            if (Random.Range(0, 100) <= spawnBoomerChanсe) {
                GameObject go = Instantiate(boomerPrefab, pos, Quaternion.identity);
                go.name = $"Slime lvl:{startLevel}, type: {SlimeType.Boomer.ToString()}";
                go.transform.localScale = Vector3.one * startLevel;
                go.GetComponent<Slime>().Initialize(startMaxHealth, startDamage, startLevel, SlimeType.Boomer);
            }
            else {
                GameObject go = Instantiate(regularPrefab, pos, Quaternion.identity);
                go.name = $"Slime lvl:{startLevel}, type: {SlimeType.Regular.ToString()}";
                go.transform.localScale = Vector3.one * startLevel;
                go.GetComponent<Slime>().Initialize(startMaxHealth, startDamage, startLevel);
            }
        }
    }

    void BeginSpawn() {
        canSpawn = true;
    }
}
