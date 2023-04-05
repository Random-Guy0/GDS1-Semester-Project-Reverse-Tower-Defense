using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int monsterCost;

    public void SpawnMonster()
    {
        if (manaManager.ableToCost(monsterCost))
        {
            Vector3 spawnPos = pathManager.GetStart().transform.position;
            spawnPos.y = 1.5f;
            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        }
    }
}
