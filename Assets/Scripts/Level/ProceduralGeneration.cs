using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    [Header("Loading")]
    [SerializeField] private bool enableLoadingBar;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Image loadingBarMask;

    [Header("Other")]
    [SerializeField] private CharacterPositioner characterPositioner;

    [Header("Overrides")]
    [SerializeField] private int overrideWidth;
    [SerializeField] private int overrideHeight;
    [SerializeField] private float overrideSeed;

    private int width;
    private int height;
    private float seed;
    private int rowsCompleted;
    private bool mapGenCompleted;
    private bool mapGenStarted;

    private int[,] map;

    private void Start()
    {
        width = overrideWidth != 0 ? overrideWidth : SettingsManager.mapWidth;
        height = overrideHeight != 0 ? overrideHeight : SettingsManager.mapHeight;
        seed = overrideSeed != 0 ? overrideSeed : SettingsManager.mapSeed;

        perlinHeightArray = new int[width];
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        IsReady = false;

        if(!mapGenStarted)
        {
            if(enableLoadingBar)
            {
                sceneLoader.SetText("Loading Terrain...");
                sceneLoader.EnableLoadingScreen(false);
                yield return sceneLoader.ReachedTargetAlpha();
                yield return new WaitForSeconds(2f);
            }
            tileMap.ClearAllTiles();
            map = GenerateArray(width, height, true);
            map = GenerateTerrain(map);
        }

        yield return mapGenCompleted;

        SmoothMap(smoothAmount);
        AddMapTiles(map, groundTile, caveTile);
        GenerateBorderTiles();

        yield return new WaitForEndOfFrame();

        if(enableLoadingBar)
        {
            sceneLoader.DisableLoadingScreen();
        }

        characterPositioner.PositionCharacter();

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
        int highestX = int.MinValue;
        int highestY = int.MinValue;

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

            if(perlinHeight > highestY)
            {
                highestX = x;
                highestY = perlinHeight;
            }

            rowsCompleted++;
            if(enableLoadingBar)
            {
                loadingBarMask.fillAmount = rowsCompleted / width;
            }
        }

        highestPoint = new Vector2Int(highestX, highestY);

        mapGenCompleted = true;
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
        int x = Random.Range(2, width - 1);
        int y = Random.Range(2, perlinHeightArray[x] - 1);

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
