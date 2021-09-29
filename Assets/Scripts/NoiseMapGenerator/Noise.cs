using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, int xOffset, int yOffset)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0)
            scale = 0.0001f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.PerlinNoise((x + xOffset) / scale, (y + yOffset) / scale);
            }
        }

        return noiseMap;
    }
}
