using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int pathPiecesAvailable;
    [SerializeField] private Shader defaultShader;
    [SerializeField] private Shader outlineShader;
    [SerializeField] private int levelWidth;
    [SerializeField] private int levelDepth;
    [SerializeField] private float gridSize;
    [SerializeField] private GridTile[] grid;
    [SerializeField] private float[] heights;
    [SerializeField] private GameObject[] gridGameobjects;
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] pathTilePrefabs;
    [SerializeField] private GameObject[] borderPrefabs;
    [SerializeField] private PathSegment[] pathSegments;
    [SerializeField] private int start;
    [SerializeField] private int end;

    [SerializeField] private GameObject levelParent;
    [SerializeField] private MonsterManager monsterManager;

    [SerializeField] private TMP_Text pathSegmentText;

    private bool[] manaPositions;

    private int selectedTileIndex = 0;

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

    private void Start()
    {
        SetPathSegmentText();
    }

    //create the new grid to be used for this level
    public void CreateGrid(int newWidth, int newDepth, GridTile[] newGrid, float[] newHeights)
    {
        DestroyImmediate(levelParent);
        levelWidth = newWidth;
        levelDepth = newDepth;
        grid = newGrid;
        heights = newHeights;
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
        
        CreateBorder();
        UpdatePathTiles();
    }

    private void CreateBorder()
    {
        Vector3 endPos = Vector3.zero;
        int x = GetXFromPosition(gridGameobjects[end].transform.position);
        int z = GetZFromPosition(gridGameobjects[end].transform.position);
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
            endPos = new Vector3(levelWidth * gridSize - 0.9f, 0.5f, z * gridSize);
        }
        else if (x - 1 < 0)
        {
            endPos = new Vector3(-1.1f, 0.5f, z * gridSize);
        }
        
        for (int i = 0; i < levelWidth; i++)
        {
            Vector3 wallPos1 = new Vector3(i * gridSize, 0.5f, -1.1f);
            if (!wallPos1.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos1, Quaternion.identity, levelParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.identity, levelParent.transform);
            }

            Vector3 wallPos2 = new Vector3(i * gridSize, 0.5f, levelDepth * gridSize - 0.9f);
            if (!wallPos2.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos2, Quaternion.identity, levelParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.identity, levelParent.transform);
            }
        }

        for (int i = 0; i < levelDepth; i++)
        {
            Vector3 wallPos1 = new Vector3(-1.1f, 0.5f, i * gridSize);
            if (!wallPos1.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos1, Quaternion.Euler(Vector3.up * 90.0f), levelParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.Euler(Vector3.up * 90.0f), levelParent.transform);
            }

            Vector3 wallPos2 = new Vector3(levelWidth * gridSize - 0.9f, 0.5f, i * gridSize);
            if (!wallPos2.Equals(endPos))
            {
                Instantiate(borderPrefabs[0], wallPos2, Quaternion.Euler(Vector3.up * 90.0f), levelParent.transform);
            }
            else
            {
                Instantiate(borderPrefabs[1], endPos, Quaternion.Euler(Vector3.up * 90.0f), levelParent.transform);
            }
        }

        Instantiate(borderPrefabs[2], new Vector3(-1.1f, 0.5f, -1.1f), Quaternion.Euler(Vector3.up * -90.0f), levelParent.transform);
        Instantiate(borderPrefabs[2], new Vector3(levelWidth * gridSize - 0.9f, 0.5f, levelDepth * gridSize - 0.9f), Quaternion.Euler(Vector3.up * 90.0f), levelParent.transform);
        Instantiate(borderPrefabs[2], new Vector3(levelWidth * gridSize - 0.9f, 0.5f, -1.1f), Quaternion.Euler(Vector3.up * 180.0f), levelParent.transform);
        Instantiate(borderPrefabs[2], new Vector3(-1.1f, 0.5f, levelDepth * gridSize - 0.9f), Quaternion.identity, levelParent.transform);
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
        return grid[GetIndexFromPosition(position)];
    }

    //uses a world position to set a point in the grid
    private void SetGridPoint(Vector3 position, GridTile newGridTile)
    {
        int x = GetXFromPosition(position);
        int z = GetZFromPosition(position);
        int index = z * levelWidth + x;
        DestroyImmediate(gridGameobjects[index]);
        grid[index] = newGridTile;
        if (!(grid[index] == GridTile.Start || grid[index] == GridTile.Path || grid[index] == GridTile.End))
        {
            gridGameobjects[index] = Instantiate(tilePrefabs[(int)newGridTile],
                new Vector3(x * gridSize, tilePrefabs[(int)newGridTile].transform.position.y,
                    (levelDepth - z - 1) * gridSize),
                Quaternion.identity, levelParent.transform);
            Vector3 scale = gridGameobjects[index].transform.localScale;
            scale.y *= heights[index];
            gridGameobjects[index].transform.localScale = scale;
        }
        else
        {
            gridGameobjects[index] = Instantiate(pathTilePrefabs[0],
                new Vector3(x * gridSize, pathTilePrefabs[0].transform.position.y,
                    (levelDepth - z - 1) * gridSize),
                Quaternion.identity, levelParent.transform);
            Vector3 scale = gridGameobjects[index].transform.localScale;
            scale.y *= heights[index];
            gridGameobjects[index].transform.localScale = scale;
            pathSegments[index] = gridGameobjects[index].GetComponent<PathSegment>();
            if (grid[index] == GridTile.Start)
            {
                start = index;
            }
            else if (grid[index] == GridTile.End)
            {
                end = index;
            }
        }
    }

    private void UpdatePathTiles()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                int currentIndex = j * levelWidth + i;
                if ((int)grid[currentIndex] >= 2)
                {
                    Vector3 position = gridGameobjects[currentIndex].transform.position;
                    bool top = j + 1 < levelDepth && (int)grid[(j + 1) * levelWidth + i] >= 2;
                    bool bottom = j - 1 >= 0 && (int)grid[(j - 1) * levelWidth + i] >= 2;
                    bool left = i - 1 >= 0 && (int)grid[j * levelWidth + i - 1] >= 2;
                    bool right = i + 1 < levelWidth && (int)grid[j * levelWidth + i + 1] >= 2;

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
                            newTile = Instantiate(pathTilePrefabs[0],
                                new Vector3(position.x, pathTilePrefabs[0].transform.position.y,
                                    position.z),
                                Quaternion.identity, levelParent.transform);
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

                    PathSegment pathSegment = pathSegments[currentIndex];
                    pathSegments[currentIndex] = newTile.AddComponent<PathSegment>();
                    pathSegments[currentIndex].Clone(pathSegment);
                    AttachedNavigationNode navigationNode =
                        gridGameobjects[currentIndex].GetComponent<AttachedNavigationNode>();
                    newTile.AddComponent<AttachedNavigationNode>().Clone(navigationNode);

                    DestroyImmediate(gridGameobjects[currentIndex]);
                    gridGameobjects[currentIndex] = newTile;
                    Vector3 scale = gridGameobjects[currentIndex].transform.localScale;
                    scale.y *= heights[currentIndex];
                    gridGameobjects[currentIndex].transform.localScale = scale;
                }
            }
        }
    }

    //place a section of the path, returning if it was placed or not
    public void PlacePath(Vector3 position)
    {
        int x = GetXFromPosition(position);
        int z = GetZFromPosition(position);
        if (GetGridPoint(position) == GridTile.Ground && pathPiecesAvailable > 0)
        {
            pathPiecesAvailable--;
            SetGridPoint(position, GridTile.Path);
            SetConnectedPathSegments(x, z);
            monsterManager.PathChange();
            UpdatePathTiles();
        }
        else if (GetGridPoint(position) == GridTile.Path)
        {
            pathPiecesAvailable++;
            RemovePathSegment(x, z);
            SetGridPoint(position, GridTile.Ground);
            monsterManager.PathChange();
            UpdatePathTiles();
        }
        
        SetPathSegmentText();
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

    private void SetConnectedPathSegments(int x, int z)
    {
        PathSegment pathSegment = pathSegments[z * levelWidth + x];
        if (x + 1 < levelWidth && pathSegments[z * levelWidth + x + 1] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[z * levelWidth + x + 1]);
            pathSegments[z * levelWidth + x + 1].AddConnectedPathSegment(pathSegment);
        }
        
        if (x - 1 >= 0 && pathSegments[z * levelWidth + x - 1] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[z * levelWidth + x - 1]);
            pathSegments[z * levelWidth + x - 1].AddConnectedPathSegment(pathSegment);
        }
        
        if (z + 1 < levelDepth && pathSegments[(z + 1) * levelWidth + x] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[(z + 1) * levelWidth + x]);
            pathSegments[(z + 1) * levelWidth + x].AddConnectedPathSegment(pathSegment);
        }
        
        if (z - 1 >= 0 && pathSegments[(z - 1) * levelWidth + x] != null)
        {
            pathSegment.AddConnectedPathSegment(pathSegments[(z - 1) * levelWidth + x]);
            pathSegments[(z - 1) * levelWidth + x].AddConnectedPathSegment(pathSegment);
        }
    }

    private void RemovePathSegment(int x, int z)
    {
        PathSegment pathSegment = pathSegments[z * levelWidth + x];
        
        if (x + 1 < levelWidth && pathSegments[z * levelWidth + x + 1] != null)
        {
            pathSegment.RemoveConnectedPathSegment(pathSegments[z * levelWidth + x + 1]);
            pathSegments[z * levelWidth + x + 1].RemoveConnectedPathSegment(pathSegment);
        }
        
        if (x - 1 >= 0 && pathSegments[z * levelWidth + x - 1] != null)
        {
            pathSegment.RemoveConnectedPathSegment(pathSegments[z * levelWidth + x - 1]);
            pathSegments[z * levelWidth + x - 1].RemoveConnectedPathSegment(pathSegment);
        }
        
        if (z + 1 < levelDepth && pathSegments[(z + 1) * levelWidth + x] != null)
        {
            pathSegment.RemoveConnectedPathSegment(pathSegments[(z + 1) * levelWidth + x]);
            pathSegments[(z + 1) * levelWidth + x].RemoveConnectedPathSegment(pathSegment);
        }
        
        if (z - 1 >= 0 && pathSegments[(z - 1) * levelWidth + x] != null)
        {
            pathSegment.RemoveConnectedPathSegment(pathSegments[(z - 1) * levelWidth + x]);
            pathSegments[(z - 1) * levelWidth + x].RemoveConnectedPathSegment(pathSegment);
        }
        pathSegments[z * levelWidth + x] = null;
    }

    public PathSegment GetPathSegmentAtPosition(Vector3 position)
    {
        return pathSegments[GetIndexFromPosition(position)];
    }

    public int GetIndexFromPosition(Vector3 position)
    {
        int x = GetXFromPosition(position);
        int z = GetZFromPosition(position);
        return z * levelWidth + x;
    }

    private int GetXFromPosition(Vector3 position)
    {
        return (int)(Mathf.RoundToInt(position.x) / gridSize);
    }

    private int GetZFromPosition(Vector3 position)
    {
        return (int)(levelDepth - 1 - Mathf.RoundToInt(position.z) / gridSize);
    }

    public PathSegment GetStart()
    {
        return pathSegments[start];
    }

    public PathSegment GetEnd()
    {
        return pathSegments[end];
    }

    public void SetSelectedTile(Vector3 position)
    {
        int newIndex = GetIndexFromPosition(position);

        if (gridGameobjects[selectedTileIndex] != null)
        {
            if (grid[selectedTileIndex] == GridTile.Ground || grid[selectedTileIndex] == GridTile.Path ||
                grid[selectedTileIndex] == GridTile.Start ||
                grid[selectedTileIndex] == GridTile.End)
            {
                MeshRenderer renderer = gridGameobjects[selectedTileIndex].GetComponent<MeshRenderer>();
                Material[] mats = renderer.materials;

                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].shader = defaultShader;
                }

                renderer.materials = mats;
            }
        }

        if (grid[newIndex] == GridTile.Ground || grid[newIndex] == GridTile.Path || grid[newIndex] == GridTile.Start ||
            grid[newIndex] == GridTile.End)
        {
            MeshRenderer renderer = gridGameobjects[newIndex].GetComponent<MeshRenderer>();
            Material[] mats = renderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].shader = outlineShader;
                mats[i].SetColor("_OutlineColor", new Color(1.0f, 1.0f, 0.0f));
                mats[i].SetFloat("_OutlineWidth", 1.1f);
            }

            renderer.materials = mats;
        }

        selectedTileIndex = newIndex;
    }

    public PathSegment[] GetPathSegments()
    {
        return pathSegments;
    }

    public int GetPathSegmentIndex(PathSegment pathSegment)
    {
        int index = -1;
        for (int i = 0; i < pathSegments.Length; i++)
        {
            if (pathSegment.Equals(pathSegments[i]))
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void SetPathSegmentText()
    {
        pathSegmentText.SetText(pathPiecesAvailable.ToString());
    }

    public Vector3[] ValidMovePositions(Vector3 position)
    {
        int x = GetXFromPosition(position);
        int z = GetZFromPosition(position);
        List<Vector3> validPositions = new List<Vector3>();
        
        if (x + 1 < levelWidth && grid[z * levelWidth + x + 1] != GridTile.Mountain)
        {
            validPositions.Add(gridGameobjects[z * levelWidth + x + 1].transform.position);
        }
        
        if (x - 1 >= 0 && grid[z * levelWidth + x - 1] != GridTile.Mountain)
        {
            validPositions.Add(gridGameobjects[z * levelWidth + x - 1].transform.position);
        }
        
        if (z + 1 < levelDepth && grid[(z + 1) * levelWidth + x] != GridTile.Mountain)
        {
            validPositions.Add(gridGameobjects[(z + 1) * levelWidth + x].transform.position);
        }
        
        if (z - 1 >= 0 && grid[(z - 1) * levelWidth + x] != GridTile.Mountain)
        {
            validPositions.Add(gridGameobjects[(z - 1) * levelWidth + x].transform.position);
        }

        return validPositions.ToArray();
    }

    public float[] GetHeights()
    {
        return heights;
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
