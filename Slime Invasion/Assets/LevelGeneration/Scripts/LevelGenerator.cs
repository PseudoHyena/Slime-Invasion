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

    float[,] noiseMap;

    public NoiseSettings Settings { get { return noiseSettings; } }
    public TerrainType[] Regions { get { return regions; } }
    public float[,] NoiseMap { get { return noiseMap; } }
    public float[,] HeightMap { get; set; } = null;

    void Awake() {
        singleton = this;    
    }

    void Start() {
        GenerateLevel();
        vegetationGenerator = GetComponent<VegetationGenerator>();
        vegetationGenerator.Generate();
    }

    public void GenerateLevel() {
        noiseSettings.ValidateValues();

        noiseMap = Noise.GenerateNoiseMap(noiseSettings);
        ApplyModifications();

        DrawMesh(MeshGenerator.GenerateMesh(HeightMap), GenerateTexture());

        OnEndOfLevelGeneration?.Invoke();
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
                float currentHeight = noiseMap[y, x];
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
                noiseMap[y, x] = Mathf.Clamp01(noiseMap[y, x] - falloff[y, x]);
                HeightMap[y, x] = heightCurve.Evaluate(noiseMap[y, x]) * heightMultiplier;
            }
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color Color;
}
