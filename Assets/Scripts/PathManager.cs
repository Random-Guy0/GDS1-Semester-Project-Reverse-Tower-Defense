using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int levelWidth;
    [SerializeField] private int levelDepth;
    [SerializeField] private float gridSize;
    [SerializeField] private GridTile[] grid;
    [SerializeField] private GameObject[] gridGameobjects;
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private PathSegment[] pathSegments;
    [SerializeField] private PathSegment start;
    [SerializeField] private PathSegment end;

    [SerializeField] private GameObject levelParent;

    private bool[] manaPositions;

    private void Awake()
    {
        manaPositions = new bool[grid.Length];
        for (int i = 0; i < manaPositions.Length; i++)
        {
            manaPositions[i] = grid[i] != GridTile.Mountain;
        }

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                if (pathSegments[j * levelWidth + i] != null)
                {
                    SetConnectedPathSegments(i, j);
                }
            }
        }
    }

    //create the new grid to be used for this level
    public void CreateGrid(int newWidth, int newDepth, GridTile[] newGrid)
    {
        DestroyImmediate(levelParent);
        levelWidth = newWidth;
        levelDepth = newDepth;
        grid = newGrid;
        gridGameobjects = new GameObject[grid.Length];
        pathSegments = new PathSegment[grid.Length];

        levelParent = new GameObject("Level Parent Object");
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                int index = (levelDepth - j - 1) * levelWidth + i;
                Vector3 position = new Vector3(i * gridSize, 0, j * gridSize);
                SetGridPoint(position, grid[index]);
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

    //uses a world position to get a point from the grid
    public GridTile GetGridPoint(Vector3 position)
    {
        int x = (int)(Mathf.RoundToInt(position.x) / gridSize);
        int z = (int)(levelDepth - 1 - Mathf.RoundToInt(position.z) / gridSize);
        return grid[z * levelWidth + x];
    }

    //uses a world position to set a point in the grid
    private void SetGridPoint(Vector3 position, GridTile newGridTile)
    {
        int x = (int)(Mathf.RoundToInt(position.x) / gridSize);
        int z = (int)(levelDepth - 1 - Mathf.RoundToInt(position.z) / gridSize);
        int index = z * levelWidth + x;
        DestroyImmediate(gridGameobjects[index]);
        grid[index] = newGridTile;
        gridGameobjects[index] = Instantiate(tilePrefabs[(int)newGridTile], new Vector3(x * gridSize, 0, (levelDepth - z - 1) * gridSize),
            Quaternion.identity, levelParent.transform);
        if (grid[index] == GridTile.Start || grid[index] == GridTile.Path || grid[index] == GridTile.End)
        {
            pathSegments[index] = gridGameobjects[index].GetComponent<PathSegment>();
            if (grid[index] == GridTile.Start)
            {
                start = pathSegments[index];
            }
            else if (grid[index] == GridTile.End)
            {
                end = pathSegments[index];
            }
        }
    }

    //place a section of the path, returning if it was placed or not
    public bool PlacePath(Vector3 position)
    {
        if (GetGridPoint(position) == GridTile.Ground)
        {
            SetGridPoint(position, GridTile.Path);
            int x = (int)(Mathf.RoundToInt(position.x) / gridSize);
            int z = (int)(levelDepth - 1 - Mathf.RoundToInt(position.z) / gridSize);
            SetConnectedPathSegments(x, z);
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Vector3> GetValidManaPositions()
    {
        List<Vector3> validManaPositions = new List<Vector3>();

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                int index = (levelDepth - j - 1) * levelWidth + i;
                if (manaPositions[index])
                {
                    validManaPositions.Add(new Vector3(i * gridSize, 1.5f, j * gridSize));
                }
            }
        }

        return validManaPositions;
    }

    private void SetConnectedPathSegments(int pathSegmentX, int pathSegmentZ)
    {
        PathSegment pathSegment = pathSegments[pathSegmentZ * levelWidth + pathSegmentX];
        if (pathSegmentX + 1 < levelWidth && pathSegments[pathSegmentZ * levelWidth + pathSegmentX + 1] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[pathSegmentZ * levelWidth + pathSegmentX + 1]);
            pathSegments[pathSegmentZ * levelWidth + pathSegmentX + 1].AddConnectedPathSegment(pathSegment);
        }
        
        if (pathSegmentX - 1 >= 0 && pathSegments[pathSegmentZ * levelWidth + pathSegmentX - 1] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[pathSegmentZ * levelWidth + pathSegmentX - 1]);
            pathSegments[pathSegmentZ * levelWidth + pathSegmentX - 1].AddConnectedPathSegment(pathSegment);
        }
        
        if (pathSegmentZ + 1 < levelDepth && pathSegments[(pathSegmentZ + 1) * levelWidth + pathSegmentX] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[(pathSegmentZ + 1) * levelWidth + pathSegmentX]);
            pathSegments[(pathSegmentZ + 1) * levelWidth + pathSegmentX].AddConnectedPathSegment(pathSegment);
        }
        
        if (pathSegmentZ - 1 >= 0 && pathSegments[(pathSegmentZ - 1) * levelWidth + pathSegmentX] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[(pathSegmentZ - 1) * levelWidth + pathSegmentX]);
            pathSegments[(pathSegmentZ - 1) * levelWidth + pathSegmentX].AddConnectedPathSegment(pathSegment);
        }
    }

    public PathSegment GetPathSegmentAtPosition(Vector3 position)
    {
        int x = (int)(Mathf.RoundToInt(position.x) / gridSize);
        int z = (int)(levelDepth - 1 - Mathf.RoundToInt(position.z) / gridSize);
        return pathSegments[z * levelWidth + x];
    }

    public PathSegment GetStart()
    {
        return start;
    }

    public PathSegment GetEnd()
    {
        return end;
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
