using UnityEngine;

public class VegetationGenerator : MonoBehaviour {

    [SerializeField] Transform grass;
    [SerializeField] Transform trees;
    [SerializeField] GameObject[] grassArray;
    [SerializeField] GameObject[] treesArray;
    [SerializeField] [Range(0, 100)] int grassSpawnChance;
    [SerializeField] [Range(0, 100)] int treesSpawnChance;

    public void SpawnVegetation(int size) {
        float[,] heightMap = LevelGenerator.singleton.HeightMap;
        float[,] noiseMap = LevelGenerator.singleton.NoiseMap;

        float grassMinHeight = 0f;
        TerrainType[] regions = LevelGenerator.singleton.Regions;
        for (int i = 0; i < regions.Length; i++) {
            if (regions[i].name == "Grass") {
                grassMinHeight = regions[i - 1].height;
                break;
            }
        }

        if (grassMinHeight == 0f) {
            return;
        }

        System.Random prnd = new System.Random(LevelGenerator.singleton.Settings.seed);

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                if (noiseMap[y, x] >= grassMinHeight) {
                    if (prnd.Next(0, 100) <= grassSpawnChance) {
                        Vector3 pos = new Vector3(x - size / 2, heightMap[y, x], size / 2 - y);
                        Quaternion rot = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                        Instantiate(grassArray[prnd.Next(0, grassArray.Length)], pos, rot, grass);
                    }
                }
            }
        }

        float treesMinHeight = 0f;
        for (int i = 0; i < regions.Length; i++) {
            if (regions[i].name == "Grass") {
                treesMinHeight = regions[i - 1].height;
                break;
            }
        }

        if (treesMinHeight == 0f) {
            return;
        }

        for (int y = 0; y < size; y += 4) {
            for (int x = 0; x < size; x += 4) {
                if (noiseMap[y, x] >= treesMinHeight) {
                    if (prnd.Next(0, 100) <= treesSpawnChance) {
                        Vector3 pos = new Vector3(x - size / 2, heightMap[y, x], size / 2 - y);
                        Instantiate(treesArray[prnd.Next(0, treesArray.Length)], pos, Quaternion.identity, trees);
                    }
                }
            }
        }
    }
}
