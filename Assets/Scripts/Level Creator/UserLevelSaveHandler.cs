using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLevelSaveHandler : MonoBehaviour
{
    private static string userLevelsFolder = Application.persistentDataPath + "/User Levels";

    public static void Save(string levelName, int levelWidth, int levelDepth, GridTile[,] grid, float[,] heights, TowerType[] towers, Vector2Int[] towerPositions, float[] towerSpawnTimes, int pathPieces, int gateHealth, int startingMana, int maxMana, Camera renderCamera, RenderTexture rt)
    {
        int[] intGrid = new int[levelWidth * (levelDepth - 1) + levelWidth];
        float[] newHeights = new float[levelWidth * (levelDepth - 1) + levelWidth];
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelDepth; j++)
            {
                intGrid[j * levelWidth + i] = (int)grid[i, j];
                newHeights[j * levelWidth + i] = heights[i, j];
            }
        }
        
        int[] towerPositionsX = new int[towerPositions.Length];
        int[] towerPositionsZ = new int[towerPositions.Length];

        for (int i = 0; i < towerPositions.Length; i++)
        {
            towerPositionsX[i] = towerPositions[i].x;
            towerPositionsZ[i] = towerPositions[i].y;
        }

        int[] intTowers = new int[towers.Length];
        for (int i = 0; i < towers.Length; i++)
        {
            intTowers[i] = (int)towers[i];
        }

        UserLevel level = new UserLevel(levelWidth, levelDepth, intGrid, newHeights, intTowers, towerPositionsX, towerPositionsZ,
            towerSpawnTimes, pathPieces, gateHealth, startingMana, maxMana);

        string levelJSON = JsonUtility.ToJson(level);

        if (!System.IO.Directory.Exists(userLevelsFolder))
        {
            System.IO.Directory.CreateDirectory(userLevelsFolder);
        }

        string path = userLevelsFolder + "/" + levelName;
        
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        System.IO.File.WriteAllText(path + "/level.json", levelJSON);
        RenderTextureHandler.SaveRTToFile(rt, renderCamera, path + "/icon.png");
    }
}
