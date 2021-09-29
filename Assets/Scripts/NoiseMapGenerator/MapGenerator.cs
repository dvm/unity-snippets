using UnityEngine;

[RequireComponent(typeof(MapVisualization))]
public class MapGenerator : MonoBehaviour
{
    [Header("Map size")]
    public int mapWidth = 128;
    public int mapHeight = 128;

    [Header("Offset (seed)")]
    public Vector2Int offset = new Vector2Int(512, -512);

    [Header("Map noise")]
    public float noiseScale = 100;

    public enum VisualizationMode
    {
        Smooth,
        BlackWhite,
        QuadColor,
        Gradient
    }

    [Header("Visualization")]
    [Tooltip("Auto update in Editor")]
    public bool autoUpdate;
    public VisualizationMode mode;
    public Gradient mapColors;

    [SerializeField] private Texture2D _noiseTexture;
    public Texture2D NoiseTexture { get => _noiseTexture; }

    public void Generate()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, offset.x, offset.y);
        GenerateNoiseMapTexture(noiseMap, out _noiseTexture);

        GetComponent<MapVisualization>().ToSprite(_noiseTexture);
    }

    public void GenerateRandom(int seed)
    {
        offset = new Vector2Int(seed, seed);
        Generate();
    }

    public void GenerateNoiseMapTexture(float[,] noiseMap, out Texture2D texture)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        switch (mode)
        {
            case VisualizationMode.Smooth:
                generateSmoothTexture(noiseMap, width, height, texture);
                break;
            case VisualizationMode.BlackWhite:
                generateBlackWhiteTexture(noiseMap, width, height, texture);
                break;
            case VisualizationMode.QuadColor:
                generateQuadColorTexture(noiseMap, width, height, texture);
                break;
            case VisualizationMode.Gradient:
                generateGradientTexture(noiseMap, width, height, texture);
                break;
        }

        texture.Apply();
    }

    private void generateSmoothTexture(float[,] noiseMap, int width, int height, Texture2D texture)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, Color.Lerp(Color.black, Color.white, noiseMap[x, y]));
            }
        }

        texture.name = "SmoothNoiseMap";
    }

    private void generateBlackWhiteTexture(float[,] noiseMap, int width, int height, Texture2D texture)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, noiseMap[x, y] > 0.5f ? Color.white : Color.black);
            }
        }

        texture.name = "BlackWhiteNoiseMap";
    }

    private void generateQuadColorTexture(float[,] noiseMap, int width, int height, Texture2D texture)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (noiseMap[x, y] < 0.25f)
                    texture.SetPixel(x, y, Color.red);
                else if (noiseMap[x, y] < 0.5f)
                    texture.SetPixel(x, y, Color.green);
                else if (noiseMap[x, y] < 0.75f)
                    texture.SetPixel(x, y, Color.blue);
                else
                    texture.SetPixel(x, y, Color.black);
            }
        }

        texture.name = "QuadColorNoiseMap";
    }

    private void generateGradientTexture(float[,] noiseMap, int width, int height, Texture2D texture)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, mapColors.Evaluate(noiseMap[x, y]));
            }
        }

        texture.name = "GradientNoiseMap";
    }

}
