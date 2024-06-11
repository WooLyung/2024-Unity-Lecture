using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

public class MapSystem : MonoBehaviour
{
    public static MapSystem Instance { get; private set; }

    public GameObject player;
    public GameObject wall;
    public GameObject tree;
    public GameObject pig;

    private int width = 10, height = 10;
    private int seed = 114257;

    private int[,] map;
    private System.Random prng;

    public MapSystem()
    {
        Instance = this;
    }

    private float[,] NewNoise(int scale)
    {
        float[,] noise = new float[width, height];

        float offsetX = prng.Next(-100000, 100000);
        float offsetY = prng.Next(-100000, 100000);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float sampleX = (i + offsetX) / scale;
                float sampleY = (j + offsetY) / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noise[i, j] = perlinValue;
            }
        }

        return noise;
    }

    private void Init(float fillPercent)
    {
        map = new int[width, height];

        // init
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                map[i, j] = prng.NextDouble() < fillPercent ? 1 : 0;
    }

    private void Smooth()
    {
        Vector2Int[] neighbor =
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int count = 0;
                foreach (Vector2Int dir in neighbor)
                {
                    Vector2Int neighborPos = new Vector2Int(i, j) + dir;
                    if (0 <= neighborPos.x && neighborPos.x < width && 0 <= neighborPos.y && neighborPos.y < height)
                        if (map[neighborPos.x, neighborPos.y] == 1)
                            count++;
                }

                if (count >= 4)
                    map[i, j] = 1;
                else
                    map[i, j] = 0;
            }
        }
    }

    private void AddNoise()
    {
        float[,] noise = NewNoise(20);

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (noise[i, j] > 0.5f)
                    map[i, j] = 1;
    }

    private void GenerateThings()
    {
        float[,] noise = NewNoise(20);
        
        ThingSystem.Instance.InstantiateThing(player, new Vector2Int(-1, -1));
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] == 1)
                    ThingSystem.Instance.InstantiateThing(wall, new Vector2Int(i, j));
                else if (noise[i, j] < 0.4f && prng.NextDouble() < 0.1)
                    ThingSystem.Instance.InstantiateThing(tree, new Vector2Int(i, j));
                else if (prng.NextDouble() < 0.01)
                    ThingSystem.Instance.InstantiateThing(pig, new Vector2Int(i, j));
            }
        }
    }

    void Start()
    {
        prng = new System.Random(seed.GetHashCode());

        Init(0.25f);
        for (int t = 0; t < 5; t++)
            Smooth();
        AddNoise();

        GenerateThings();
    }
}
