using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator singleton;

    public System.Action OnEndOfLevelGeneration;

    [SerializeField] NoiseSettings noiseSettings;
    [SerializeField] [Range(1f, 50f)] float heightMultiplier;
    [SerializeField] AnimationCurve heightCurve;

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshCollider meshCollider;

    [SerializeField] TerrainType[] regions;

    VegetationGenerator vegetationGenerator;

    public NoiseSettings Settings { get { return noiseSettings; } }
    public TerrainType[] Regions { get { return regions; } }
    public float[,] NoiseMap { get; private set; }
    public float[,] HeightMap { get; private set; }

    void Awake() {
        singleton = this;    
    }

    void Start() {
        Debug.Log($"The level was loaded with seed: {MenuManager.Seed}");
        noiseSettings.seed = MenuManager.Seed;

        GenerateLevel();
        vegetationGenerator = GetComponent<VegetationGenerator>();
        vegetationGenerator.SpawnVegetation(noiseSettings.size);
    }

    public void GenerateLevel() {
        noiseSettings.ValidateValues();

        NoiseMap = Noise.GenerateNoiseMap(noiseSettings);
        ApplyModifications();

        DrawMesh(MeshGenerator.GenerateMesh(HeightMap), GenerateTexture());

        OnEndOfLevelGeneration?.Invoke();
        BeginSpawnEntity();
    }

    void DrawMesh(MeshData meshData, Texture2D texture) {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshCollider.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    Texture2D GenerateTexture() {
        Color[] colorMap = new Color[noiseSettings.size * noiseSettings.size];
        for (int y = 0; y < noiseSettings.size; y++) {
            for (int x = 0; x < noiseSettings.size; x++) {
                float currentHeight = NoiseMap[y, x];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y * noiseSettings.size + x] = regions[i].Color;
                        break;
                    }
                }
            }
        }

        Texture2D texture = new Texture2D(noiseSettings.size, noiseSettings.size);
        texture.filterMode = FilterMode.Trilinear;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    void ApplyModifications() {
        HeightMap = new float[noiseSettings.size, noiseSettings.size];
        float[,] falloff = FalloffGenerator.GenerateFalloffMap(noiseSettings.size);

        for (int y = 0; y < noiseSettings.size; y++) {
            for (int x = 0; x < noiseSettings.size; x++) {
                NoiseMap[y, x] = Mathf.Clamp01(NoiseMap[y, x] - falloff[y, x]);
                HeightMap[y, x] = heightCurve.Evaluate(NoiseMap[y, x]) * heightMultiplier;
            }
        }
    }

    void BeginSpawnEntity() {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (var item in spawners) {
            item.GetComponent<ISpawner>()?.BeginSpawn();
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color Color;
}
