using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class ProceduralTerrain : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 3;

    public string seedString; // Burada seed olarak kullanılacak string'i tanımlıyoruz

    private System.Random randomGen;

    private TerrainData terrainData;

    void Start()
    {
        int seedValue = StringToSeed(seedString);  // String seed'i integer bir değere dönüştürüyoruz
        randomGen = new System.Random(seedValue);  // Bu değeri Random sınıfını başlatmak için kullanıyoruz

        terrainData = GetComponent<Terrain>().terrainData;
        terrainData = GenerateTerrain(terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, 50, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        Vector2 randomOffset = new Vector2(randomGen.Next(-100000, 100000), randomGen.Next(-100000, 100000));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y, randomOffset);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y, Vector2 offset)
    {
        float xCoord = (float)x / width * scale + offset.x;
        float yCoord = (float)y / height * scale + offset.y;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    int StringToSeed(string s)  // String'i integer bir seed değerine dönüştüren fonksiyon
    {
        int seed = 0;
        foreach (char c in s)
        {
            seed = (seed * 997) + (int)c;
        }
        return seed;
    }
}
