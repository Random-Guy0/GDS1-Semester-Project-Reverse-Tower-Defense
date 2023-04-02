using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int levelWidth;
    [SerializeField] private int levelDepth;
    [SerializeField] private int gridSize;
    [SerializeField] private GridTile[] grid;
    [SerializeField] private GameObject[] gridGameobjects;
    [SerializeField] private GameObject[] tilePrefabs;

    private GameObject levelParent;

    //create the new grid to be used for this level
    public void CreateGrid(int newWidth, int newDepth, GridTile[] newGrid)
    {
        DestroyImmediate(levelParent);
        levelWidth = newWidth;
        levelDepth = newDepth;
        grid = newGrid;

        levelParent = new GameObject("Level Parent Object");
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                int index = (levelDepth - j - 1) * levelWidth + i;
                Vector3 position = new Vector3(i * gridSize, 0, j * gridSize);
                Instantiate(tilePrefabs[(int)grid[index]], position, Quaternion.identity, levelParent.transform);
            }
        }

        GameObject border1 = Instantiate(tilePrefabs[tilePrefabs.Length - 1], new Vector3(-1.5f, 0, (levelDepth * gridSize) / 2.0f - 1.0f), Quaternion.identity, levelParent.transform);
        Vector3 border1Scale = border1.transform.localScale;
        border1Scale.z = levelDepth * gridSize + 2.0f;
        border1.transform.localScale = border1Scale;
        
        GameObject border2 = Instantiate(tilePrefabs[tilePrefabs.Length - 1], new Vector3(levelWidth * gridSize - 0.5f, 0, (levelDepth * gridSize) / 2.0f - 1.0f), Quaternion.identity, levelParent.transform);
        Vector3 border2Scale = border2.transform.localScale;
        border2Scale.z = levelDepth * gridSize + 2.0f;
        border2.transform.localScale = border2Scale;
        
        GameObject border3 = Instantiate(tilePrefabs[tilePrefabs.Length - 1], new Vector3((levelWidth * gridSize) / 2.0f - 1.0f, 0, -1.5f), Quaternion.identity, levelParent.transform);
        Vector3 border3Scale = border3.transform.localScale;
        border3Scale.x = levelWidth * gridSize + 2.0f;
        border3.transform.localScale = border3Scale;
        
        GameObject border4 = Instantiate(tilePrefabs[tilePrefabs.Length - 1], new Vector3((levelWidth * gridSize) / 2.0f - 1.0f, 0, levelDepth * gridSize - 0.5f), Quaternion.identity, levelParent.transform);
        Vector3 border4Scale = border4.transform.localScale;
        border4Scale.x = levelWidth * gridSize + 2.0f;
        border4.transform.localScale = border4Scale;
    }

    //delete the grid for this level
    public void ResetGrid()
    {
        levelWidth = 0;
        levelDepth = 0;
        grid = Array.Empty<GridTile>();
        DestroyImmediate(levelParent);
    }

    public int GetLevelWidth()
    {
        return levelWidth;
    }

    public int GetLevelDepth()
    {
        return levelDepth;
    }

    public GridTile[] GetGrid()
    {
        return grid;
    }
}

public enum GridTile
{
    Ground,
    Mountain,
    Path,
    Start,
    End
}
