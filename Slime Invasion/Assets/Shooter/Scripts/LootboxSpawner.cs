using UnityEngine;

public class LootboxSpawner : MonoBehaviour, ISpawner {

    [SerializeField] GameObject lootbox;
    [SerializeField] float spawnRate;
    [SerializeField] int maxCount;

    public static int Count { get; set; }

    bool canSpawn = false;

    float nextSpawn;

    Transform player;

    void Start() {
        //LevelGenerator.singleton.OnEndOfLevelGeneration += BeginSpawn;
    }

    void Update() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        Spawn();    
    }

    public void Spawn() {
        if (canSpawn && Count < maxCount && Time.time > nextSpawn) {
            nextSpawn = Time.time + spawnRate;

            Instantiate(lootbox, player.position + new Vector3(Random.Range(1f, 2f), 20f, Random.Range(-2f, -1f)), Quaternion.identity, transform);

            Count++;
        }
    }

    public void BeginSpawn() {
        canSpawn = true;
    }
}
