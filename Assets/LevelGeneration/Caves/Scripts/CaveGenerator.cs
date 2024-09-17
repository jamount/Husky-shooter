using UnityEngine;
using System.Collections;

public class CaveGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColourMap, Mesh, CaveMap};
    public DrawMode drawMode;

    const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;


    public bool autoUpdate;

    public TerrainType[] caveRegions;

    float[,] falloffMap;

    public void GenerateMap()
    {
        float[,] caveNoiseMap = Noise.GenerateSymmetricalNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        float[,] caveMap = PerlinWorm.GenerateMapFromNoise(mapChunkSize, caveNoiseMap, 0.6f, 0.8f);
        float[,] plateauNoiseMap = Noise.GenerateSymmetricalNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale * 0.5f, octaves, persistance, lacunarity, offset);

        float[,] plataueMap = PerlinWorm.GenerateMapFromNoise(mapChunkSize, plateauNoiseMap, 0.4f, 0.6f);


        float[,] floorNoise = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale / 10, octaves, persistance, lacunarity, offset);


        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                caveMap[x, y] = Mathf.Clamp(caveMap[x, y] + plataueMap[x, y] * 0.2f, 0, 1);
                caveMap[x, y] = Mathf.Clamp(caveMap[x, y] + floorNoise[x, y] / 5, 0, 1);


                caveMap[x, y] = Mathf.Clamp(caveMap[x, y] / 1f + falloffMap[x, y], 0.01f, 1);






                float currentHeight = caveMap[x, y];
                for (int i = 0; i < caveRegions.Length; i++)
                {
                    if (currentHeight <= caveRegions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = caveRegions[i].colour;
                        break;
                    }
                }
            }
        }



        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(caveNoiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            var mesh = display.DrawMesh(MeshGenerator.GenerateTerrainMesh(caveMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.CaveMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(caveMap));
        }
    }
    

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        falloffMap = FalloffGenerator.GenerateCircularFalloffMap(mapChunkSize, 0.8f, 1.6f);

    }

    private GameObject GetRandomObject(GameObject[] tileArray)
    {
        return tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
    }
}


