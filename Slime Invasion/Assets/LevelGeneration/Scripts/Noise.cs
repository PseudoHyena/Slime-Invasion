using UnityEngine;

//Class generating noise map
public static class Noise {

	public static float[,] GenerateNoiseMap(NoiseSettings settings) {
        int size = settings.size;
        float scale = settings.scale;
        int seed = settings.seed;
        int octaves = settings.octaves;

        float persistance = settings.persistance;
        float lacunarity = settings.lacunarity;

        float[,] noiseMap = new float[size, size];

        System.Random prnd = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++) {
            octavesOffset[i].x = prnd.Next(-100000, 100000);
            octavesOffset[i].y = prnd.Next(-100000, 100000);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x + octavesOffset[i].x) / scale * frequency;
                    float sampleY = (y + octavesOffset[i].y) / scale * frequency;

                    noiseHeight += (Mathf.PerlinNoise(sampleY, sampleX) * 2 - 1) * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxLocalNoiseHeight) {
                    maxLocalNoiseHeight = noiseHeight;
                }

                if (noiseHeight < minLocalNoiseHeight) {
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap[y, x] = noiseHeight;
            }
        }

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                noiseMap[y, x] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[y, x]);
            }
        }

        return noiseMap;
    }
}

[System.Serializable]
public class NoiseSettings {
    public int size = 100;
    public float scale = 43f;
    public int octaves = 4;

    [Range(0f, 1f)] public float persistance = 0.5f;
    public float lacunarity = 3f;

    public int seed = 0;

    public void ValidateValues() {
        scale = Mathf.Max(scale, 0.01f);
        octaves = Mathf.Max(octaves, 1);
        lacunarity = Mathf.Max(lacunarity, 1f);
        persistance = Mathf.Clamp01(persistance);

        if (seed == 0) {
            seed = System.DateTime.Now.Millisecond;
        }
    }
}
