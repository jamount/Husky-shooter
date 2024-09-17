using System.Linq;
using UnityEngine;

public static class StructureMapGenerator
{
    public static float[,] GenerateStructureMap(int size, out Vector2Int[] structurePoints, float circleRadius = 8.0f, float falloffMultiplier = 1.0f)
    {
        float[,] map = new float[size, size];

        // Define the center points for each of the three circles
        float centerX = size / 2f; // Center X is the same for all circles
        float centerTopY = size / 6f; // Top circle's Y position
        float centerMiddleY = size / 2f; // Middle circle's Y position
        float centerBottomY = 5 * size / 6f; // Bottom circle's Y position

        // Store structure points as Vector2Int
        var structurePoints_f = new Vector2[3]
        {
            new Vector2(centerX, centerTopY),
            new Vector2(centerX, centerMiddleY),
            new Vector2(centerX, centerBottomY)
        };
        structurePoints = structurePoints_f.Select(x => Vector2Int.RoundToInt(x)).ToArray();

        // Loop through each point in the map
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // Calculate distances to each circle's center
                float dx = i - centerX;

                float dyTop = j - centerTopY;
                float dyMiddle = j - centerMiddleY;
                float dyBottom = j - centerBottomY;

                // Calculate distances from the point to the center of each circle
                double distanceToTopCircle = Mathf.Sqrt(dx * dx + dyTop * dyTop);
                double distanceToMiddleCircle = Mathf.Sqrt(dx * dx + dyMiddle * dyMiddle);
                double distanceToBottomCircle = Mathf.Sqrt(dx * dx + dyBottom * dyBottom);

                // Calculate falloff values based on distance
                float falloffValue = 0.0f;

                if (distanceToTopCircle <= circleRadius)
                {
                    falloffValue = Mathf.Clamp01(1 - (float)(distanceToTopCircle / circleRadius)); // Falloff for top circle
                }
                else if (distanceToMiddleCircle <= circleRadius)
                {
                    falloffValue = Mathf.Clamp01(1 - (float)(distanceToMiddleCircle / circleRadius)); // Falloff for middle circle
                }
                else if (distanceToBottomCircle <= circleRadius)
                {
                    falloffValue = Mathf.Clamp01(1 - (float)(distanceToBottomCircle / circleRadius)); // Falloff for bottom circle
                }

                // Set the falloff value in the map
                map[i, j] = Mathf.Pow(falloffValue, falloffMultiplier);
            }
        }

        return map;
    }
}