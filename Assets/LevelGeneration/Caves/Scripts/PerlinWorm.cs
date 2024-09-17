using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinWorm
{
    public static float[,] GenerateMapFromNoise(int chunkSize, float[,] noiseMap, float lowThreshold, float highThreshold)
    {
        float[,] map = new float[chunkSize, chunkSize];

        for (int i = 0; i < chunkSize; i++)
        {
            for(int j = 0; j < chunkSize; j++)
            {
                if (noiseMap[i, j] >= lowThreshold && noiseMap[i, j] <= highThreshold)
                {
                    map[i, j] = 0f;
                }

                // If we are between the thresholds, smoothly interpolate
                else
                {
                    map[i, j] = Mathf.InverseLerp(highThreshold, 1, noiseMap[i, j]) * 5;
                }

            }
        }

        return map;
    }
}
