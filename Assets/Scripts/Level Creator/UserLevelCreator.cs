using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UserLevelCreator : MonoBehaviour
{
    private const int gridSize = 2;
    
    private GridTile[,] grid;
    private GridTile[,] defaultGrid;

    private int levelWidth;
    private int levelDepth;

    [SerializeField] private int defaultLevelWidth;
    [SerializeField] private int defaultLevelDepth;

    private float[,] heights;
    private float[,] defaultHeights;

    private int start = -1;
    private int end = -1;

    private GameObject levelParent;
    private GameObject[] gridGameobjects;
    
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] pathTilePrefabs;
    [SerializeField] private GameObject[] borderPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;

    private void Awake()
    {
        defaultGrid = new GridTile[defaultLevelWidth, defaultLevelDepth];
        defaultHeights = new float[defaultLevelWidth, defaultLevelDepth];
        for (int i = 0; i < defaultLevelWidth; i++)
        {
            for (int j = 0; j < defaultLevelDepth; j++)
            {
                defaultGrid[i, j] = GridTile.Ground;
                defaultHeights[i, j] = 1f;
            }
        }

        levelWidth = defaultLevelWidth;
        levelDepth = defaultLevelDepth;

        grid = defaultGrid;
        heights = defaultHeights;
        
        CreateGrid();
    }

    private void CreateGrid()
    {
        levelParent = new GameObject("Level Parent Object");
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                SetGridPoint(i, j);
            }
        }
    }

    private void SetGridPoint(int x, int z)
    {
        GridTile tileToInstantiate = grid[x, z];
        GameObject objectToInstantiate;
        Vector3 rotation = Vector3.zero;
        if ((int)tileToInstantiate < 2)
        {
            objectToInstantiate = tilePrefabs[(int)tileToInstantiate];
        }
        else if ((int)tileToInstantiate >= 2 && (int)tileToInstantiate < 5)
        {
            objectToInstantiate = pathTilePrefabs[0];
        }
        else
        {
            objectToInstantiate = obstaclePrefabs[(int)tileToInstantiate - 5];
            rotation = Vector3.up;
            int randRotation = Random.Range(0, 4);
            rotation.y *= 90.0f * randRotation;
        }

        Vector3 position = new Vector3(x * gridSize, objectToInstantiate.transform.position.y,
            (levelDepth - z - 1) * gridSize);

        GameObject newTile =
            Instantiate(objectToInstantiate, position, Quaternion.Euler(rotation), levelParent.transform);
    }

    public Vector3 GetCenter()
    {
        float x = levelWidth * gridSize * 0.5f;
        float z = levelDepth * gridSize * 0.5f;
        return new Vector3(x, 0, z);
    }

    public int GetWidth()
    {
        return levelWidth * gridSize;
    }

    public void SetSelectedTile(Vector3 position)
    {
        
    }
}
