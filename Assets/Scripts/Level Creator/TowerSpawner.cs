using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefabs;

    public void SpawnTowers(TowerType[] towers, Vector2Int[] towerPositions, float[] towerSpawnTimes, PathManager pathManager)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            GameObject tower = Instantiate(towerPrefabs[(int)towers[i]],
                new Vector3(towerPositions[i].x  * pathManager.GetGridSize(), pathManager.MeshHeight(towerPositions[i].y * pathManager.GetLevelWidth() + towerPositions[i].x) + 1, (pathManager.GetLevelDepth() - towerPositions[i].y - 1)  * pathManager.GetGridSize()),
                Quaternion.identity);

            Tower towerClass = tower.GetComponent<Tower>();
            towerClass.spawnTime = towerSpawnTimes[i];
        }
    }
}
