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

    [SerializeField] private float stepHeight = 0.5f;
    private float[,] heights;
    private float[,] defaultHeights;

    private Vector2Int start = Vector2Int.one * -1;
    private Vector2Int end = Vector2Int.one * -1;

    private GameObject levelParent;
    private GameObject borderParent;
    private GameObject[,] gridGameobjects;

    private Vector2Int selectedTile = Vector2Int.one * -1;

    private int selectedTool = 0;

    [SerializeField] private UIWarning uiWarning;
    
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] pathTilePrefabs;
    [SerializeField] private GameObject[] borderPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;

    private void Awake()
    {
        defaultGrid = new GridTile[defaultLevelWidth, defaultLevelDepth];
        defaultHeights = new float[defaultLevelWidth, defaultLevelDepth];
        gridGameobjects = new GameObject[defaultLevelWidth, defaultLevelDepth];
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
        levelParent.layer = 14;
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                SetGridPoint(i, j);
            }
        }
        CreateBorder();
        UpdatePathTiles();
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

        gridGameobjects[x, z] =
            Instantiate(objectToInstantiate, position, Quaternion.Euler(rotation), levelParent.transform);
        gridGameobjects[x, z].layer = 14;
        
        SetLayerInChildren(gridGameobjects[x, z]);
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
        selectedTile = GetIndexFromPosition(position);
    }
    
    public Vector2Int GetIndexFromPosition(Vector3 position)
    {
        int x = GetXFromPosition(position);
        int z = GetZFromPosition(position);
        return new Vector2Int(x, z);
    }

    private int GetXFromPosition(Vector3 position)
    {
        return (int)(Mathf.RoundToInt(position.x) / gridSize);
    }

    private int GetZFromPosition(Vector3 position)
    {
        return (int)(levelDepth - 1 - Mathf.RoundToInt(position.z) / gridSize);
    }
    
    private void CreateBorder()
    {
        Destroy(borderParent);
        borderParent = new GameObject("Border Parent Object");
        borderParent.layer = 14;

        Vector3 endPos = Vector3.one * -1f;
        if (!end.Equals(Vector2Int.one * -1))
        {
            int x = GetXFromPosition(gridGameobjects[end.x, end.y].transform.position);
            int z = GetZFromPosition(gridGameobjects[end.x, end.y].transform.position);
            if (z + 1 >= levelDepth)
            {
                endPos = new Vector3(x * gridSize, 0.5f, -1.1f);
            }
            else if (z - 1 < 0)
            {
                endPos = new Vector3(x * gridSize, 0.5f, levelDepth * gridSize - 0.9f);
            }
            else if (x + 1 >= levelWidth)
            {
                endPos = new Vector3(levelWidth * gridSize - 0.9f, 0.5f, (levelDepth - z - 1) * gridSize);
            }
            else if (x - 1 < 0)
            {
                endPos = new Vector3(-1.1f, 0.5f, (levelDepth - z - 1) * gridSize);
            }
        }

        for (int i = 0; i < levelWidth; i++)
        {
            Vector3 wallPos1 = new Vector3(i * gridSize, 0.5f, -1.1f);
            if (!wallPos1.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos1, Quaternion.identity, borderParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.identity, borderParent.transform);
            }

            Vector3 wallPos2 = new Vector3(i * gridSize, 0.5f, levelDepth * gridSize - 0.9f);
            if (!wallPos2.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos2, Quaternion.identity, borderParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.identity, borderParent.transform);
            }
        }

        for (int i = 0; i < levelDepth; i++)
        {
            Vector3 wallPos1 = new Vector3(-1.1f, 0.5f, i * gridSize);
            if (!wallPos1.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos1, Quaternion.Euler(Vector3.up * 90.0f), borderParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.Euler(Vector3.up * 90.0f), borderParent.transform);
            }

            Vector3 wallPos2 = new Vector3(levelWidth * gridSize - 0.9f, 0.5f, i * gridSize);
            if (!wallPos2.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos2, Quaternion.Euler(Vector3.up * 90.0f), borderParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.Euler(Vector3.up * 90.0f), borderParent.transform);
            }
        }

        Instantiate(borderPrefabs[2], new Vector3(-1.1f, 0.5f, -1.1f), Quaternion.Euler(Vector3.up * -90.0f), borderParent.transform);
        Instantiate(borderPrefabs[2], new Vector3(levelWidth * gridSize - 0.9f, 0.5f, levelDepth * gridSize - 0.9f), Quaternion.Euler(Vector3.up * 90.0f), borderParent.transform);
        Instantiate(borderPrefabs[2], new Vector3(levelWidth * gridSize - 0.9f, 0.5f, -1.1f), Quaternion.Euler(Vector3.up * 180.0f), borderParent.transform);
        Instantiate(borderPrefabs[2], new Vector3(-1.1f, 0.5f, levelDepth * gridSize - 0.9f), Quaternion.identity, borderParent.transform);

        SetLayerInChildren(borderParent);
    }
    
    private void UpdatePathTiles()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                if ((int)grid[i, j] >= 2 && (int)grid[i, j] < 5)
                {
                    Vector3 position = gridGameobjects[i, j].transform.position;
                    bool top = j + 1 < levelDepth && (int)grid[i, j + 1] >= 2 && (int)grid[i, j + 1] < 5 && Mathf.Abs(MeshHeight(i, j) - MeshHeight(i, j + 1)) <= stepHeight;
                    bool bottom = j - 1 >= 0 && (int)grid[i, j - 1] >= 2 && (int)grid[i, j - 1] < 5 && Mathf.Abs(MeshHeight(i, j) - MeshHeight(i, j - 1)) <= stepHeight;
                    bool left = i - 1 >= 0 && (int)grid[i - 1, j] >= 2 && (int)grid[i - 1, j] < 5 && Mathf.Abs(MeshHeight(i, j) - MeshHeight(i - 1, j)) <= stepHeight;
                    bool right = i + 1 < levelWidth && (int)grid[i + 1, j] >= 2 && (int)grid[i + 1, j] < 5 && Mathf.Abs(MeshHeight(i, j) - MeshHeight(i + 1, j)) <= stepHeight;

                    int countTrue = 0;
                    if (top)
                    {
                        countTrue++;
                    }

                    if (bottom)
                    {
                        countTrue++;
                    }

                    if (left)
                    {
                        countTrue++;
                    }

                    if (right)
                    {
                        countTrue++;
                    }

                    Vector3 rotation = Vector3.zero;
                    GameObject newTile;

                    switch (countTrue)
                    {
                        case 0:
                            if (i + 1 >= levelWidth)
                            {
                                rotation.y = -90f;
                            }
                            else if (i - 1 < 0)
                            {
                                rotation.y = 90f;
                            }
                            else if (j - 1 < 0)
                            {
                                rotation.y = 180f;
                            }
                            
                            newTile = Instantiate(pathTilePrefabs[0],
                                new Vector3(position.x, pathTilePrefabs[0].transform.position.y,
                                    position.z),
                                Quaternion.Euler(rotation), levelParent.transform);
                            break;
                        
                        case 1:
                            if (left)
                            {
                                rotation.y = -90f;
                            }
                            else if (right)
                            {
                                rotation.y = 90f;
                            }
                            else if (top)
                            {
                                rotation.y = 180f;
                            }
                            newTile = Instantiate(pathTilePrefabs[0],
                                new Vector3(position.x, pathTilePrefabs[0].transform.position.y,
                                    position.z),
                                Quaternion.Euler(rotation), levelParent.transform);
                            break;

                        case 2:
                            if ((top && bottom) || (left && right))
                            {
                                if (left && right)
                                {
                                    rotation.y = 90f;
                                }
                                
                                newTile = Instantiate(pathTilePrefabs[1],
                                    new Vector3(position.x, pathTilePrefabs[1].transform.position.y,
                                        position.z),
                                    Quaternion.Euler(rotation), levelParent.transform);
                            }
                            else
                            {
                                if (bottom && right)
                                {
                                    rotation.y = 90f;
                                }
                                else if (top && left)
                                {
                                    rotation.y = -90f;
                                }
                                else if (top && right)
                                {
                                    rotation.y = 180f;
                                }
                                newTile = Instantiate(pathTilePrefabs[2],
                                    new Vector3(position.x, pathTilePrefabs[2].transform.position.y,
                                        position.z),
                                    Quaternion.Euler(rotation), levelParent.transform);
                            }

                            break;

                        case 3:
                            if (!left)
                            {
                                rotation.y = 90f;
                            }
                            else if (!right)
                            {
                                rotation.y = -90f;
                            }
                            else if (!bottom)
                            {
                                rotation.y = 180f;
                            }
                            newTile = Instantiate(pathTilePrefabs[3],
                                new Vector3(position.x, pathTilePrefabs[3].transform.position.y,
                                    position.z),
                                Quaternion.Euler(rotation), levelParent.transform);
                            break;

                        case 4:
                            newTile = Instantiate(pathTilePrefabs[4],
                                new Vector3(position.x, pathTilePrefabs[4].transform.position.y,
                                    position.z),
                                Quaternion.identity, levelParent.transform);
                            break;
                        default:
                            newTile = new GameObject();
                            break;
                    }

                    Destroy(gridGameobjects[i, j]);
                    gridGameobjects[i, j] = newTile;
                    Vector3 scale = gridGameobjects[i, j].transform.localScale;
                    scale.y *= heights[i, j];
                    gridGameobjects[i, j].transform.localScale = scale;
                }
            }
        }
    }
    
    private float MeshHeight(int x, int z)
    {
        return gridGameobjects[x, z].GetComponent<MeshRenderer>().bounds.size.y;
    }

    public void SetTool(int tool)
    {
        selectedTool = tool;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !selectedTile.Equals(Vector2Int.one * -1))
        {
            MouseDown();
        }
    }

    private void MouseDown()
    {
        switch (selectedTool)
        {
            //selection tool
            case 0:
                break;
            //ground tool
            case 1:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.Ground);
                break;
            //mountain tool
            case 2:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.Mountain);
                break;
            //path tool
            case 3:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.Path);
                break;
            //start tool
            case 4:
                if (start.Equals(Vector2Int.one * -1))
                {
                    start = selectedTile;
                    UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.Start);
                }
                else if (!selectedTile.Equals(start))
                {
                    uiWarning.SetActive("Start already exists!");
                }
                break;
            //end tool
            case 5:
                if (end.Equals(Vector2Int.one * -1))
                {
                    end = selectedTile;
                    UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.End);
                }
                else if (!selectedTile.Equals(end))
                {
                    uiWarning.SetActive("End already exists!");
                }
                break;
            //archery range obstacle tool
            case 6:
                break;
            //barracks obstacle tool
            case 7:
                break;
            //castle obstacle tool
            case 8:
                break;
            //farm obstacle tool
            case 9:
                break;
            //forest obstacle tool
            case 10:
                break;
            //house obstacle tool
            case 11:
                break;
            //lumbermill obstacle tool
            case 12:
                break;
            //market obstacle tool
            case 13:
                break;
            //mill obstacle tool
            case 14:
                break;
            //mine obstacle tool
            case 15:
                break;
            //mountain obstacle tool
            case 16:
                break;
            //rocks obstacle tool
            case 17:
                break;
            //watchtower obstacle tool
            case 18:
                break;
            //watermill obstacle tool
            case 19:
                break;
            //well obstacle tool
            case 20:
                break;
        }
    }

    private void UpdateGridPoint(int x, int z, GridTile newTile)
    {
        if (grid[x, z] != newTile)
        {
            if (start.x == x && start.y == z && newTile != GridTile.Start)
            {
                start = Vector2Int.one * -1;
                uiWarning.SetActive("Start tile has been removed!");
            }
            
            if (end.x == x && end.y == z && newTile != GridTile.End)
            {
                end = Vector2Int.one * -1;
                uiWarning.SetActive("End tile has been removed!");
            }
            
            grid[x, z] = newTile;
            Destroy(gridGameobjects[x, z]);
            SetGridPoint(x, z);
            UpdatePathTiles();
            CreateBorder();
        }
    }

    public void ClearSelectedTile(Vector3 position)
    {
        if (selectedTile == GetIndexFromPosition(position))
        {
            selectedTile = Vector2Int.one * -1;
        }
    }

    private void SetLayerInChildren(GameObject parentObject)
    {
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            parentObject.transform.GetChild(i).gameObject.layer = 14;
            SetLayerInChildren(parentObject.transform.GetChild(i).gameObject);
        }
    }
}
