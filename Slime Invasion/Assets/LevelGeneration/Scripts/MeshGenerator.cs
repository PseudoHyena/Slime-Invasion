using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

	public static MeshData GenerateMesh(float[,] heightMap) {
        int size = heightMap.GetLength(0);

        float topLeftX = (size - 1) / -2f;
        float topLeftZ = (size - 1) / 2f;

        MeshData meshData = new MeshData(size);
        int vertexIndex = 0;

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[y, x], topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)size, y / (float)size);

                if (x < size - 1 && y < size - 1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + size + 1, vertexIndex + size);
                    meshData.AddTriangle(vertexIndex + size + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int size) {
        vertices = new Vector3[size * size];
        uvs = new Vector2[size * size];
        triangles = new int[(size - 1) * (size - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c) {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

}