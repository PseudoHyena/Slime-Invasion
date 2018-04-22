using UnityEngine;

public class LootboxSpawner : MonoBehaviour {

    [SerializeField] GameObject lootbox;
    [SerializeField] float spawnRate;
    [SerializeField] int maxCount;

    bool canSpawn = false;

    float nextSpawn;
    int count;

    Transform player;

    void Start() {
        LevelGenerator.singleton.OnEndOfLevelGeneration += BeginSpawn;
    }

    void Update() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        Spawn();    
    }

    void Spawn() {
        if (canSpawn && count < maxCount && Time.time > nextSpawn) {
            nextSpawn = Time.time + spawnRate;

            Instantiate(lootbox, player.position + new Vector3(Random.Range(1f, 2f), 10f, Random.Range(-2f, -1f)), Quaternion.identity, transform);
        }
    }

    void BeginSpawn() {
        canSpawn = true;
    }
}
