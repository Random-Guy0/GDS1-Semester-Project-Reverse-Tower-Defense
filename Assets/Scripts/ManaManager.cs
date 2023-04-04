using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManaManager : MonoBehaviour
{
    public int currentMana;
    public int maxMana;
    public int minMana;

    [SerializeField] private PathManager pathManager;
    [SerializeField] private GameObject manaPrefab;
    [SerializeField] private float timeToSpawn;
    [SerializeField] private int maxToSpawn;
    private int numberCurrentlySpawned = 0;

    private List<Vector3> validManaSpawns;

    private void Awake()
    {
        maxMana = 100;
        minMana = 0;
    }

    private void Start()
    {
        validManaSpawns = pathManager.GetValidManaPositions();
        SpawnMana();
    }

    public int getMana() 
    {
        return currentMana; 
    }

    public void CollectMana(Vector3 position)
    {
        validManaSpawns.Add(position);
        addMana(50);
        numberCurrentlySpawned--;
    }
    
    public void addMana(int mana)
    {
        if (currentMana + mana >= maxMana)
        {
            currentMana = maxMana;
            return;
        }
        currentMana = currentMana + mana;
    }

    public void costMana(int cost)
    {
        currentMana -= cost;
    }

    public bool ableToCost(int cost)
    {
        return currentMana - cost >= minMana;
    }

    private void SpawnMana()
    {
        if (numberCurrentlySpawned < maxToSpawn && validManaSpawns.Count != 0)
        {
            int chosenIndex = Random.Range(0, validManaSpawns.Count);
            Vector3 chosenPosition = validManaSpawns[chosenIndex];
            validManaSpawns.RemoveAt(chosenIndex);
            Instantiate(manaPrefab, chosenPosition, Quaternion.identity);
        }
        
        Invoke("SpawnMana", timeToSpawn);
    }
}
