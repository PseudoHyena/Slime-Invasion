using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField] NoiseSettings noiseSettings;
    [SerializeField] [Range(1f, 50f)] float heightMultiplier;
    [SerializeField] AnimationCurve heightCurve;

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshCollider meshCollider;

    [SerializeField] TerrainType[] regions;

    float[,] noiseMap;

    void Start() {
        GenerateLevel();    
    }

    public void GenerateLevel() {
        noiseSettings.ValidateValues();

        noiseMap = Noise.GenerateNoiseMap(noiseSettings);
        ApplyFalloff();

        DrawMesh(MeshGenerator.GenerateMesh(noiseMap, heightMultiplier, heightCurve), GenerateTexture());
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

    void ApplyFalloff() {
        float[,] falloff = FalloffGenerator.GenerateFalloffMap(noiseSettings.size);

        for (int y = 0; y < noiseSettings.size; y++) {
            for (int x = 0; x < noiseSettings.size; x++) {
                noiseMap[y, x] = Mathf.Clamp01(noiseMap[y, x] - falloff[y, x]);
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
