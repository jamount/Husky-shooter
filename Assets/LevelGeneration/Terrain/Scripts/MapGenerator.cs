using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColourMap, Mesh, FallOffMap, StructureMap };
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

    public enum Falloff { None, Normal, Inverse}
    public Falloff falloff;
    float[,] falloffMap;


    float[,] structureMap;
    Vector2Int[] StructurePoints;
    public GameObject[] Structures;


    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (falloff == Falloff.Normal)
                {
                    noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] / 1.5f - falloffMap[x, y], 0, 1);
    
                    noiseMap[x, y] = (noiseMap[x, y] + structureMap[x, y]) / 4;
                }
                if (falloff == Falloff.Inverse)
                {
                    noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] / 1.5f + falloffMap[x, y], 0, 1);
                    noiseMap[x, y] /= 1 + structureMap[x, y];
                    //if (structureMap[x,y] > 0.1f)
                    //{
                    //    noiseMap[x, y] = Mathf.Lerp(noiseMap[x, y], noiseMap[x, y]  + structureMap[x, y], 0.5f);

                    //}
                }

                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }



        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            var mesh = display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));

            foreach (var point in StructurePoints)
            {
                var position = new Vector3(point.x - mapChunkSize / 2, meshHeightMultiplier * meshHeightCurve.Evaluate(noiseMap[point.x, point.y]), point.y - mapChunkSize / 2);

                var structure = Instantiate(GetRandomObject(Structures), position, Quaternion.identity, mesh.transform);
            }
        }
        else if (drawMode == DrawMode.FallOffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, 2.2f, 1f)));
        }
        else if (drawMode == DrawMode.StructureMap)
        {
            if (falloff == Falloff.Inverse)
            {
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(StructureMapGenerator.GenerateStructureMap(mapChunkSize, out StructurePoints, 24, 0.8f)));
            }

            if (falloff == Falloff.Normal)
            {
                {
                    display.DrawTexture(TextureGenerator.TextureFromHeightMap(StructureMapGenerator.GenerateStructureMap(mapChunkSize, out StructurePoints, 24, 0.8f)));
                }

            }
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

        if (falloff == Falloff.Inverse)
        {
            falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, 2.2f, 1f);
            structureMap = StructureMapGenerator.GenerateStructureMap(mapChunkSize, out StructurePoints, 24, 0.8f);
        }

        if (falloff == Falloff.Normal)
        {
            falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, 2.2f, 0.8f);
            structureMap = StructureMapGenerator.GenerateStructureMap(mapChunkSize, out StructurePoints, 24, 0.8f);
        }

    }

    private GameObject GetRandomObject(GameObject[] tileArray)
    {
        return tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
    }
}





[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}