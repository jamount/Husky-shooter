using System;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int size, float xFalloffMultiplier = 1.0f, float yFalloffMultiplier = 1.0f)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // Normalize x and y to the range [-1, 1]
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                // Apply different falloff multipliers for the x and y axes
                x *= xFalloffMultiplier;
                y *= yFalloffMultiplier;

                // Calculate the maximum of the scaled absolute values
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value);

            }
        }

        return map;
    }

    public static float[,] GenerateDiamondFalloffMap(int size, float xFalloffMultiplier = 1.0f, float yFalloffMultiplier = 1.0f)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // Normalize x and y to the range [-1, 1]
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                // Apply different falloff multipliers for the x and y axes
                x *= xFalloffMultiplier;
                y *= yFalloffMultiplier;

                // Calculate the sum of the absolute values (Manhattan distance) for diamond shape
                float value = Mathf.Abs(x) + Mathf.Abs(y);

                // Clamp value between 0 and 1 to avoid overflow
                value = Mathf.Clamp01(value);

                // Apply the falloff function
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    public static float[,] GenerateCircularFalloffMap(int size, float xFalloffMultiplier = 1.0f, float yFalloffMultiplier = 1.0f)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // Normalize x and y to the range [-1, 1]
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                // Apply different falloff multipliers for the x and y axes
                x *= xFalloffMultiplier;
                y *= yFalloffMultiplier;

                // Calculate the Euclidean distance from the center (for circular shape)
                float value = Mathf.Sqrt(x * x + y * y);

                // Clamp value between 0 and 1 to keep it within the desired range
                value = Mathf.Clamp01(value);

                // Apply the falloff function
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Abs(Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a)));
    }
}