using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] GameObject regularPrefab;
    [SerializeField] GameObject boomerPrefab;

    [SerializeField][Range(0, 100)] int spawnBoomerChanсe; 

    [SerializeField] float spawnRate = 20f;

    [SerializeField] int startMaxHealth = 100;
    [SerializeField] int startDamage = 50;
    [SerializeField] float startLevel = 1f;

    [SerializeField] int smallSlimesMaxCount = 30;
    [SerializeField] int mediumSlimesMaxCount = 20;
    [SerializeField] int bigSlimesMaxCount = 10;

    public static int SmallSlimesCount { get; set; } = 0;
    public static int MediumSlimesCount { get; set; } = 0;
    public static int BigSlimesCount { get; set; } = 0;


    bool canSpawn = false;
    float nextSpawn;

    void Start() {
        LevelGenerator.singleton.OnEndOfLevelGeneration += BeginSpawn;    
    }

    void Update() {
        Spawn();
    }

    void OnValidate() {
        if (startLevel != 1f && startLevel != 0.5f && startLevel != 0.25f) {
            startLevel = 1f;
        }    

        if (startLevel < 1f && spawnBoomerChanсe > 0) {
            spawnBoomerChanсe = 0;
        }
    }

    void Spawn() {
        if (canSpawn && Time.time >= nextSpawn) {
            if (startLevel == 1f) {
                if (BigSlimesCount == bigSlimesMaxCount) {
                    return;
                } 
            }
            else if (startLevel == 0.5f) {
                if (MediumSlimesCount == mediumSlimesMaxCount) {
                    return;
                }
            }
            else {
                if (SmallSlimesCount == smallSlimesMaxCount) {
                    return;
                }
            }

            nextSpawn = Time.time + spawnRate;

            Vector3 pos = transform.position
                    + new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 1.5f), Random.Range(-1f, 1f));

            pos.x = Mathf.Clamp(pos.x, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.z = Mathf.Clamp(pos.z, -GameManager.GameFieldLength, GameManager.GameFieldLength);

            if (Random.Range(0, 100) <= spawnBoomerChanсe) {
                GameObject go = Instantiate(boomerPrefab, pos, Quaternion.identity, transform);
                go.name = $"Slime lvl:{startLevel}, type: {SlimeType.Boomer.ToString()}";
                go.transform.localScale = Vector3.one * startLevel;
                go.GetComponent<Slime>().Initialize(startMaxHealth, startDamage, startLevel, SlimeType.Boomer);
            }
            else {
                GameObject go = Instantiate(regularPrefab, pos, Quaternion.identity, transform);
                go.name = $"Slime lvl:{startLevel}, type: {SlimeType.Regular.ToString()}";
                go.transform.localScale = Vector3.one * startLevel;
                go.GetComponent<Slime>().Initialize(startMaxHealth, startDamage, startLevel);
            }

            if (startLevel == 1f) {
                BigSlimesCount++;
            }
            else if (startLevel == 0.5f) {
                MediumSlimesCount++;
            }
            else {
                SmallSlimesCount++;
            }
        }
    }

    void BeginSpawn() {
        canSpawn = true;
    }
}
