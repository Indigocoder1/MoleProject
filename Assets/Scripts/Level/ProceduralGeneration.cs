using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [HideInInspector]
    public static bool IsReady = false;

    [Header("Terrain Generation")]
    [SerializeField] private int borderWidth;
    [SerializeField] private float xSmooth;
    [SerializeField] private int ySmooth;
    private int[] perlinHeightArray;
    private Vector2Int highestPoint;

    [Header("Cave Generation")]
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    [SerializeField] private int smoothAmount;

    [Header("Tiles")]
    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase caveTile;
    [SerializeField] private TileBase borderTile;
    [SerializeField] private Tilemap tileMap;

    [Header("Overrides")]
    [SerializeField] private int overrideWidth;
    [SerializeField] private int overrideHeight;
    [SerializeField] private float overrideSeed;

    private int width;
    private int height;
    private float seed;

    private int[,] map;

    private void Start()
    {
        width = overrideWidth != 0 ? overrideWidth : SettingsManager.mapWidth;
        height = overrideHeight != 0 ? overrideHeight : SettingsManager.mapHeight;
        seed = overrideSeed != 0? overrideSeed : SettingsManager.mapSeed;

        perlinHeightArray = new int[width];
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        IsReady = false;

        tileMap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = GenerateTerrain(map);
        SmoothMap(smoothAmount);
        AddMapTiles(map, groundTile, caveTile);
        GenerateBorderTiles();
        highestPoint = FindHighestPoint();

        yield return new WaitForEndOfFrame();

        IsReady = true;
    }

    private int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = empty ? 0 : 1;
            }
        }

        return map;
    }

    private int[,] GenerateTerrain(int[,] map)
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / xSmooth, seed) * height / ySmooth);
            perlinHeight += height / ySmooth;
            perlinHeightArray[x] = perlinHeight;
            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = pseudoRandom.Next(1, 100) < randomFillPercent ? 1 : 2;
            }
        }

        return map;
    }

    private void GenerateBorderTiles()
    {
        for (int x = -borderWidth; x < 0; x++)
        {
            for (int y = 0; y < perlinHeightArray[0]; y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), borderTile);
            }
        }

        for (int x = - borderWidth; x < width + borderWidth; x++)
        {
            for (int y = -borderWidth; y < 0; y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), borderTile);
            }
        }

        for (int x = width; x < width + borderWidth; x++)
        {
            for (int y = 0; y < perlinHeightArray[width - 1]; y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), borderTile);
            }
        }
    }

    private void SmoothMap(int smoothAmount)
    {
        for (int i = 0; i < smoothAmount; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < perlinHeightArray[x]; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perlinHeightArray[x] - 1 || y == perlinHeightArray[x] - 2)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        int surroundingGroundCount = GetSurroundingGroundCount(x, y);
                        if (surroundingGroundCount > 4)
                        {
                            map[x, y] = 1;
                        }
                        else if (surroundingGroundCount < 4)
                        {
                            map[x, y] = 2;
                        }
                    }
                }
            }
        }
    }

    private int GetSurroundingGroundCount(int gridX, int gridY)
    {
        int neighborGroundCount = 0;
        for (int nebX = gridX - 1; nebX <= gridX + 1; nebX++)
        {
            for (int nebY = gridY - 1; nebY <= gridY + 1; nebY++)
            {
                if (nebX == gridX && nebY == gridY)
                {
                    continue;
                }

                if (nebX >= 0 && nebX < width && nebY >= 0 && nebY < height)
                {
                    if (map[nebX, nebY] == 1)
                    {
                        neighborGroundCount++;
                    }
                }
            }
        }

        return neighborGroundCount;
    }

    private void AddMapTiles(int[,] map, TileBase groundTileBase, TileBase caveTileBase)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
                }
                else if (map[x, y] == 2)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), caveTileBase);
                }
            }
        }
    }

    private Vector2Int FindHighestPoint()
    {
        int highestY = int.MinValue;
        int highestX = int.MinValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < perlinHeightArray[x]; y++)
            {
                if (map[x,y] == 1 && y > highestY)
                {
                    highestY = y;
                    highestX = x;
                }
            }
        }

        return new Vector2Int(highestX, highestY);
    }

    public Vector2Int GetHighestPoint()
    {
        return highestPoint;
    }

    public int[] GetPerlinHeightArray()
    {
        return perlinHeightArray;
    }

    public Vector2Int GetMapSize()
    {
        return new Vector2Int(width, height);
    }

    public Vector2Int GetSpawnPosition()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, perlinHeightArray[x]);

        return new Vector2Int(x, y);
    }

    public void SetTile(Vector3Int position, bool isGround)
    {
        if(isGround)
        {
            tileMap.SetTile(position, groundTile);
        }
        else if (map[position.x, position.y] == 1)
        {
            tileMap.SetTile(position, caveTile);
        }
    }
}
