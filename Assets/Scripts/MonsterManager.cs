using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private int[] monsterCosts;

    private List<Monster> monsters;

    private void Start()
    {
        monsters = new List<Monster>();
    }

    public void SpawnMonster(int type)
    {
        if (manaManager.ableToCost(monsterCosts[type]))
        {
            manaManager.costMana(monsterCosts[type]);
            Vector3 spawnPos = pathManager.GetStart().transform.position;
            spawnPos.y = 1.5f;
            monsters.Add(Instantiate(monsterPrefabs[type], spawnPos, Quaternion.identity).GetComponent<Monster>());
        }
    }

    public void MonsterDeath(Monster monster)
    {
        monsters.Remove(monster);
    }

    public void PathChange()
    {
        foreach (Monster monster in monsters)
        {
            monster.GeneratePath();
        }
    }
}
