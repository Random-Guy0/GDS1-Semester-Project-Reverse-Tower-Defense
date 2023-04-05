using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class ManaManager : MonoBehaviour
{
    public static ManaManager instance;
    public TMP_Text currentManaCount;

    public int currentMana;
    public int maxMana;
    public int minMana;

    [SerializeField] private int manaPerCollection;

    [SerializeField] private PathManager pathManager;
    [SerializeField] private GameObject manaPrefab;
    [SerializeField] private float timeToSpawn;
    [SerializeField] private int maxToSpawn;
    private int numberCurrentlySpawned = 0;

    private List<Vector3> validManaSpawns;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    	currentManaCount.text = "Mana: " + currentMana.ToString();
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
        addMana(manaPerCollection);
        numberCurrentlySpawned--;
    }
    
    public void addMana(int mana)
    {
        if (currentMana + mana >= maxMana)
        {
            currentMana = maxMana;
            currentManaCount.text = "Mana: " + currentMana.ToString();
            return;
        }
        currentMana = currentMana + mana;
        currentManaCount.text = "Mana: " + currentMana.ToString();
    }

    public void costMana(int cost)
    {
        currentMana -= cost;
        currentManaCount.text = "Mana: " + currentMana.ToString();
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
            numberCurrentlySpawned++;
        }
        
        Invoke("SpawnMana", timeToSpawn);
    }
}
