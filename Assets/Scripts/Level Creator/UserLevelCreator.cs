using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UserLevelCreator : MonoBehaviour
{
    private string levelName;
    [SerializeField] private TMP_Text levelNameText;
    
    private float gridSize = 2f;
    
    private GridTile[,] grid;
    private GridTile[,] defaultGrid;

    private int levelWidth;
    private int levelDepth;

    [SerializeField] private int defaultLevelWidth;
    [SerializeField] private int defaultLevelDepth;

    [SerializeField] private int pathPieces;
    [SerializeField] private int gateHealth;
    [SerializeField] private int startingMana;
    [SerializeField] private int maxMana;

    [SerializeField] private float stepHeight = 0.5f;
    private float[,] heights;
    private float[,] defaultHeights;

    private Vector2Int start = Vector2Int.one * -1;
    private Vector2Int end = Vector2Int.one * -1;

    private GameObject levelParent;
    private GameObject borderParent;
    private GameObject[,] gridGameobjects;

    private List<TowerType> towers;
    private List<Vector2Int> towerPositions;
    private List<float> towerSpawnTimes;
    private List<GameObject> towerGameobjects;

    private Vector2Int selectedTile = Vector2Int.one * -1;

    private int selectedTool = 0;
    
    public bool CanSelect { get; private set; }

    [SerializeField] private UIWarning uiWarning;
    
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] pathTilePrefabs;
    [SerializeField] private GameObject[] borderPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject[] towerPrefabs;

    [SerializeField] private TMP_InputField tileHeightInput;
    [SerializeField] private TMP_InputField towerSpawnTimeInput;
    [SerializeField] private Canvas selectionPopupCanvas;
    [SerializeField] private GameObject selectionPopup;

    [SerializeField] private GameObject towerSpawnTimeSettings;
    [SerializeField] private GameObject towerDeleteButton;

    [SerializeField] private Camera renderCamera;
    [SerializeField] private RenderTexture rt;

    [SerializeField] private TMP_InputField pathPiecesInput;
    [SerializeField] private TMP_InputField gateHealthInput;
    [SerializeField] private TMP_InputField startingManaInput;
    [SerializeField] private TMP_InputField maxManaInput;

    private void Awake()
    {
        LevelName levelName = FindObjectOfType<LevelName>();

        if (levelName != null && System.IO.File.Exists(Application.persistentDataPath + "/User Levels/" + levelName.GetLevelName() + "/level.json"))
        {
            string levelJSON =
                System.IO.File.ReadAllText(Application.persistentDataPath + "/User Levels/" + levelName.GetLevelName() +
                                           "/level.json");
            UserLevel level = JsonUtility.FromJson<UserLevel>(levelJSON);

            defaultLevelWidth = level.levelWidth;
            defaultLevelDepth = level.levelDepth;
            defaultGrid = new GridTile[defaultLevelWidth, defaultLevelDepth];
            defaultHeights = new float[defaultLevelWidth, defaultLevelDepth];
            gridGameobjects = new GameObject[defaultLevelWidth, defaultLevelDepth];
            for (int i = 0; i < defaultLevelWidth; i++)
            {
                for (int j = 0; j < defaultLevelDepth; j++)
                {
                    defaultGrid[i, j] = (GridTile)level.grid[j * defaultLevelWidth + i];
                    defaultHeights[i, j] = level.heights[j * defaultLevelWidth + i];
                }
            }

            int[] towerIntArray = level.towers;
            towers = new List<TowerType>();

            for (int i = 0; i < towerIntArray.Length; i++)
            {
                towers.Add((TowerType)towerIntArray[i]);
            }

            int[] towerPositionsX = level.towerPositionsX;
            int[] towerPositionsZ = level.towerPositionsZ;
            
            towerPositions = new List<Vector2Int>();
            for (int i = 0; i < towerPositionsX.Length; i++)
            {
                towerPositions.Add(new Vector2Int(towerPositionsX[i], towerPositionsZ[i]));
            }
            
            towerSpawnTimes = new List<float>(level.towerSpawnTimes);

            pathPieces = level.pathPieces;
            gateHealth = level.gateHealth;
            startingMana = level.startingMana;
            maxMana = level.maxMana;
        }
        else
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
            towers = new List<TowerType>();
            towerPositions = new List<Vector2Int>();
            towerSpawnTimes = new List<float>();
        }
        
        towerGameobjects = new List<GameObject>();

        levelWidth = defaultLevelWidth;
        levelDepth = defaultLevelDepth;

        grid = defaultGrid;
        heights = defaultHeights;
        
        CreateGrid();

        for (int i = 0; i < towers.Count; i++)
        {
            SpawnTower(towerPositions[i].x, towerPositions[i].y, towers[i]);
        }

        CanSelect = true;

        pathPiecesInput.text = pathPieces.ToString();
        gateHealthInput.text = gateHealth.ToString();
        startingManaInput.text = startingMana.ToString();
        maxManaInput.text = maxMana.ToString();
    }

    public void SetLevelName(string levelName)
    {
        this.levelName = levelName;
        levelNameText.SetText(levelName);
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
            if (tileToInstantiate == GridTile.Start)
            {
                start = new Vector2Int(x, z);
            }
            else if (tileToInstantiate == GridTile.End)
            {
                end = new Vector2Int(x, z);
            }
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
    }

    private void ScaleGridPoint(int x, int z)
    {
        float prefabHeight = 1f;
        if (grid[x, z] == GridTile.Mountain)
        {
            prefabHeight = 4f;
        }
        
        Vector3 scale = gridGameobjects[x, z].transform.localScale;
        scale.y = prefabHeight * heights[x, z];
        gridGameobjects[x, z].transform.localScale = scale;

        for (int i = 0; i < gridGameobjects[x, z].transform.childCount; i++)
        {
            Transform child = gridGameobjects[x, z].transform.GetChild(i);
            Vector3 childScale = child.transform.localScale;
            childScale.y /= heights[x, z];
            child.transform.localScale = childScale;
        }
    }

    public Vector3 GetCenter()
    {
        float x = levelWidth * gridSize * 0.5f;
        float z = levelDepth * gridSize * 0.5f;
        return new Vector3(x, 0, z);
    }

    public float GetWidth()
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
            int x = end.x;
            int z = end.y;
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
        if (selectedTool == 0)
        {
            ClearSelection();
        }
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
                if (!selectedTile.Equals(Vector2Int.one * -1))
                {
                    Select(selectedTile.x, selectedTile.y);
                }

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
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.ArcheryRangeObstacle);
                break;
            //barracks obstacle tool
            case 7:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.BarracksObstacle);
                break;
            //castle obstacle tool
            case 8:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.CastleObstacle);
                break;
            //farm obstacle tool
            case 9:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.FarmObstacle);
                break;
            //forest obstacle tool
            case 10:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.ForestObstacle);
                break;
            //house obstacle tool
            case 11:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.HouseObstacle);
                break;
            //lumbermill obstacle tool
            case 12:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.LumbermillObstacle);
                break;
            //market obstacle tool
            case 13:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.MarketObstacle);
                break;
            //mill obstacle tool
            case 14:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.MillObstacle);
                break;
            //mine obstacle tool
            case 15:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.MineObstacle);
                break;
            //mountain obstacle tool
            case 16:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.MountainObstacle);
                break;
            //rocks obstacle tool
            case 17:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.RocksObstacle);
                break;
            //watchtower obstacle tool
            case 18:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.WatchtowerObstacle);
                break;
            //watermill obstacle tool
            case 19:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.WatermillObstacle);
                break;
            //well obstacle tool
            case 20:
                UpdateGridPoint(selectedTile.x, selectedTile.y, GridTile.WellObstacle);
                break;
            //archer tower tool
            case 21:
                PlaceTower(selectedTile.x, selectedTile.y, TowerType.Archer);
                break;
            //knight tower tool
            case 22:
                PlaceTower(selectedTile.x, selectedTile.y, TowerType.Knight);
                break;
            //wizard tower tool
            case 23:
                PlaceTower(selectedTile.x, selectedTile.y, TowerType.Wizard);
                break;
            //ballista tower tool
            case 24:
                PlaceTower(selectedTile.x, selectedTile.y, TowerType.Ballista);
                break;
        }
    }

    private void Select(int x, int z)
    {
        if (CanSelect)
        {
            CanSelect = false;

            selectionPopup.SetActive(true);
            Vector2 positon;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(selectionPopupCanvas.transform as RectTransform,
                Input.mousePosition, selectionPopupCanvas.worldCamera, out positon);

            selectionPopup.transform.position = selectionPopupCanvas.transform.TransformPoint(positon);

            tileHeightInput.text = gridGameobjects[x, z].transform.localScale.y.ToString();

            RectTransform selectionTransform = selectionPopup.transform as RectTransform;

            int towerIndex = CheckForTower(x, z);
            if (towerIndex != -1)
            {
                towerSpawnTimeInput.text = towerSpawnTimes[towerIndex].ToString();
                towerSpawnTimeSettings.SetActive(true);
                towerDeleteButton.SetActive(true);
                selectionTransform.sizeDelta = new Vector2(200f, 210f);
            }
            else
            {
                towerSpawnTimeSettings.SetActive(false);
                towerDeleteButton.SetActive(false);
                selectionTransform.sizeDelta = new Vector2(200f, 110f);
            }
        }
    }

    public void SetHeight(string heightString)
    {
        float height = ValidateTileHeightInput(heightString);

        float prefabHeight = 1f;
        if (grid[selectedTile.x, selectedTile.y] == GridTile.Mountain)
        {
            prefabHeight = 4f;
        }

        heights[selectedTile.x, selectedTile.y] = height / prefabHeight;
        Debug.Log(heights[selectedTile.x, selectedTile.y]);
        ScaleGridPoint(selectedTile.x, selectedTile.y);
    }

    private float ValidateTileHeightInput(string tileHeightString)
    {
        float tileHeight = float.Parse(tileHeightString);
        if (tileHeight < 0.5f)
        {
            uiWarning.SetActive("Tile height cannot be less than 0.5!");
            tileHeightInput.text = "0.5";
            tileHeight = 0.5f;
        }
        else if (tileHeight > 5.0f)
        {
            uiWarning.SetActive("Tile height cannot be greater than 5!");
            tileHeightInput.text = "5";
            tileHeight = 5f;
        }

        return tileHeight;
    }

    private float ValidateTowerSpawnTimeInput(string spawnTimeString)
    {
        float spawnTime = float.Parse(spawnTimeString);
        if (spawnTime < 0f)
        {
            uiWarning.SetActive("Tower spawn time must be positive!");
            towerSpawnTimeInput.text = "0";
            spawnTime = 0f;
        }

        return spawnTime;
    }

    public void SetTowerSpawnTime(string spawnTimeString)
    {
        float spawnTime = ValidateTowerSpawnTimeInput(spawnTimeString);
        int towerIndex = CheckForTower(selectedTile.x, selectedTile.y);
        if (towerIndex != -1)
        {
            towerSpawnTimes[towerIndex] = spawnTime;
        }
    }

    public void SetPathPieces(string pathPiecesString)
    {
        int newPathPieces = int.Parse(pathPiecesString);
        if (newPathPieces < 0)
        {
            uiWarning.SetActive("Path pieces must be positive!");
            newPathPieces = 0;
            pathPiecesInput.text = "0";
        }
        
        pathPieces = newPathPieces;
    }

    public void SetGateHealth(string gateHealthString)
    {
        int newGateHealth = int.Parse(gateHealthString);
        if (newGateHealth < 1)
        {
            uiWarning.SetActive("Gate health must be 1 or greater!");
            newGateHealth = 1;
            gateHealthInput.text = "1";
        }
        
        gateHealth = newGateHealth;
    }

    public void SetStartingMana(string startingManaString)
    {
        int newStartingMana = int.Parse(startingManaString);
        if (newStartingMana < 0)
        {
            uiWarning.SetActive("Starting mana must be positive!");
            newStartingMana = 0;
            startingManaInput.text = "0";
        }
        
        startingMana = newStartingMana;

        if (startingMana > maxMana)
        {
            maxMana = startingMana;
            maxManaInput.text = maxMana.ToString();
        }
    }

    public void SetMaxMana(string maxManaString)
    {
        int newMaxMana = int.Parse(maxManaString);
        if (newMaxMana < 20)
        {
            uiWarning.SetActive("Max mana must be 20 or greater!");
            newMaxMana = 20;
            maxManaInput.text = "20";
        }
        
        maxMana = newMaxMana;

        if (maxMana < startingMana)
        {
            startingMana = maxMana;
            startingManaInput.text = startingMana.ToString();
        }
    }

    public void DeleteTower()
    {
        int towerIndex = CheckForTower(selectedTile.x, selectedTile.y);
        if (towerIndex != -1)
        {
            RemoveTower(towerIndex);
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

            if (grid[x, z] == GridTile.Mountain)
            {
                int towerIndex = CheckForTower(x, z);
                if(towerIndex != -1)
                {
                    RemoveTower(towerIndex);
                }
            }
            
            grid[x, z] = newTile;
            Destroy(gridGameobjects[x, z]);
            SetGridPoint(x, z);
            UpdatePathTiles();
            CreateBorder();
        }
    }

    public void ClearSelection()
    {
        CanSelect = true;

        if (!selectedTile.Equals(Vector2Int.one * -1))
        {
            gridGameobjects[selectedTile.x, selectedTile.y].GetComponent<SelectTile>().ClearSelection();
        }
        
        selectionPopup.SetActive(false);
    }

    public void ClearSelectedTile(Vector3 position)
    {
        if (selectedTile == GetIndexFromPosition(position))
        {
            selectedTile = Vector2Int.one * -1;
        }
    }

    private void PlaceTower(int x, int z, TowerType tower)
    {
        if (grid[x, z] == GridTile.Mountain)
        {
            if (!CheckForTowerAndReplace(x, z, tower))
            {
                towers.Add(tower);
                towerPositions.Add(new Vector2Int(x, z));
                towerSpawnTimes.Add(30f);

                SpawnTower(x, z, tower);
            }
        }
        else
        {
            uiWarning.SetActive("Towers can only be placed on mountains!");
        }
    }

    private void SpawnTower(int x, int z, TowerType tower)
    {
        GameObject newTower = Instantiate(towerPrefabs[(int)tower],
            new Vector3(x  * gridSize, MeshHeight(x, z) + 1, (levelDepth - z - 1)  * gridSize),
            towerPrefabs[(int)tower].transform.rotation);
        towerGameobjects.Add(newTower);
    }

    private void RemoveTower(int index)
    {
        Destroy(towerGameobjects[index]);
        towerGameobjects.RemoveAt(index);
        towers.RemoveAt(index);
        towerPositions.RemoveAt(index);
        towerSpawnTimes.RemoveAt(index);
    }

    private bool CheckForTowerAndReplace(int x, int z, TowerType tower)
    {
        bool exists = false;
        for (int i = 0; i < towerPositions.Count; i++)
        {
            if (towerPositions[i].x == x && towerPositions[i].y == z)
            {
                if (towers[i] == tower)
                {
                    exists = true;
                }
                else
                {
                    RemoveTower(i);
                }

                break;
            }
        }

        return exists;
    }

    private int CheckForTower(int x, int z)
    {
        int index = -1;
        for (int i = 0; i < towerPositions.Count; i++)
        {
            if (towerPositions[i].x == x && towerPositions[i].y == z)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void IncreaseLevelWidth()
    {
        levelWidth++;
        grid = ArrayHelper.ResizeArray(grid, levelWidth, levelDepth);
        gridGameobjects = ArrayHelper.ResizeArray(gridGameobjects, levelWidth, levelDepth);
        heights = ArrayHelper.ResizeArray(heights, levelWidth, levelDepth);
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                if (heights[i, j] == 0f)
                {
                    heights[i, j] = 1f;
                }
            }
        }
        
        for (int i = 0; i < levelDepth; i++)
        {
            SetGridPoint(levelWidth - 1, i);
        }
        CreateBorder();
    }

    public void DecreaseLevelWidth()
    {
        if (levelWidth > 2)
        {
            for (int i = 0; i < levelDepth; i++)
            {
                Destroy(gridGameobjects[levelWidth - 1, i]);
                int towerIndex = CheckForTower(levelWidth - 1, i);
                if (towerIndex != -1)
                {
                    RemoveTower(towerIndex);
                }
            }

            levelWidth--;
            grid = ArrayHelper.RemoveColumnFromArray(grid, levelWidth);
            gridGameobjects = ArrayHelper.RemoveColumnFromArray(gridGameobjects, levelWidth);
            heights = ArrayHelper.RemoveColumnFromArray(heights, levelWidth);
            if (start.x == levelWidth)
            {
                start = Vector2Int.one * -1;
            }

            if (end.x == levelWidth)
            {
                end = Vector2Int.one * -1;
            }
            CreateBorder();
        }
        else
        {
            uiWarning.SetActive("You cannot make the width of your level smaller!");
        }
    }

    public void IncreaseLevelDepth()
    {
        levelDepth++;
        grid = ArrayHelper.InsertRowIntoArray(grid, 0);
        gridGameobjects = ArrayHelper.InsertRowIntoArray(gridGameobjects, 0);
        heights = ArrayHelper.InsertRowIntoArray(heights, 0);
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                if (heights[i, j] == 0f)
                {
                    heights[i, j] = 1f;
                }
            }
        }
        
        for (int i = 0; i < towerPositions.Count; i++)
        {
            Vector2Int pos = towerPositions[i];
            pos.y += 1;
            towerPositions[i] = pos;
        }

        if (!start.Equals(Vector2Int.one * -1))
        {
            start.y += 1;
        }

        if (!end.Equals(Vector2Int.one * -1))
        {
            end.y += 1;
        }

        for (int i = 0; i < levelWidth; i++)
        {
            SetGridPoint(i, 0);
        }
        CreateBorder();
    }

    public void DecreaseLevelDepth()
    {
        if (levelDepth > 2)
        {
            for (int i = 0; i < levelWidth; i++)
            {
                Destroy(gridGameobjects[i, 0]);
                int towerIndex = CheckForTower(i, 0);
                if (towerIndex != -1)
                {
                    RemoveTower(towerIndex);
                }
            }

            for (int i = 0; i < towerPositions.Count; i++)
            {
                Vector2Int pos = towerPositions[i];
                pos.y -= 1;
                towerPositions[i] = pos;
            }

            levelDepth--;
            grid = ArrayHelper.RemoveRowFromArray(grid, 0);
            gridGameobjects = ArrayHelper.RemoveRowFromArray(gridGameobjects, 0);
            heights = ArrayHelper.RemoveRowFromArray(heights, 0);
            if (start.y == 0)
            {
                start = Vector2Int.one * -1;
            }
            else if(!start.Equals(Vector2Int.one * -1))
            {
                start.y -= 1;
            }

            if (end.y == 0)
            {
                end = Vector2Int.one * -1;
            }
            else if(!end.Equals(Vector2Int.one * -1))
            {
                end.y -= 1;
            }
            CreateBorder();
        }
        else
        {
            uiWarning.SetActive("You cannot make the depth of your level smaller!");
        }
    }

    public bool Save()
    {
        if (start.Equals(Vector2Int.one * -1))
        {
            uiWarning.SetActive("You must have one start tile in your level!");
            return false;
        }

        if (end.Equals(Vector2Int.one * -1))
        {
            uiWarning.SetActive("You must have one end tile in your level!");
            return false;
        }
        
        renderCamera.transform.position = new Vector3(-levelWidth, levelWidth + levelDepth, -levelDepth);
        renderCamera.transform.LookAt(GetCenter());
        UserLevelSaveHandler.Save(levelName, levelWidth, levelDepth, grid, heights, towers.ToArray(), towerPositions.ToArray(), towerSpawnTimes.ToArray(), pathPieces, gateHealth, startingMana, maxMana, renderCamera, rt);
        return true;
    }

    public void Quit()
    {
        if (Save())
        {
            SceneManager.LoadScene("UserLevels");
        }
    }

    public void Play()
    {
        if (Save())
        {
            GameObject levelNameObject = new GameObject("Level Name");
            LevelName levelNameComponent = levelNameObject.AddComponent<LevelName>();
            levelNameComponent.SetLevelName(levelName);
            DontDestroyOnLoad(levelNameObject);
            SceneManager.LoadScene("UserLevelPlayer");
        }
    }
}