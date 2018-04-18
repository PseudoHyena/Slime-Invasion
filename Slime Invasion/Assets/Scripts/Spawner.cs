using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] GameObject prefab;
    [SerializeField] float spawnRate = 20f;

    [SerializeField] int startMaxHealth = 100;
    [SerializeField] int startDamage = 50;
    [SerializeField] float startLevel = 1f;

    float nextSpawn;

    void Update() {
        Spawn();
    }
    
    void Spawn() {
        if (Time.time >= nextSpawn) {
            nextSpawn = Time.time + spawnRate;

            Vector3 pos = transform.position
                    + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

            pos.x = Mathf.Clamp(pos.x, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.z = Mathf.Clamp(pos.z, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.y = 1f;

            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            go.name = $"Slime lvl:{startLevel}";
            go.transform.localScale = Vector3.one * startLevel;
            go.GetComponent<Slime>().Initialize(startMaxHealth, startDamage, startLevel);
        }
    }
}
