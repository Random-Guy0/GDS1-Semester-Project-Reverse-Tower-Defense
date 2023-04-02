using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int levelWidth;
    [SerializeField] private int levelDepth;
    [SerializeField] private GridTile[] grid;
    [SerializeField] private GameObject[] gridGameobjects;

    //create the new grid to be used for this level
    public void CreateGrid(int newWidth, int newDepth, GridTile[] newGrid)
    {
        levelWidth = newWidth;
        levelDepth = newDepth;
        grid = newGrid;
    }

    //delete the grid for this level
    public void ResetGrid()
    {
        levelWidth = 0;
        levelDepth = 0;
        grid = Array.Empty<GridTile>();
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
