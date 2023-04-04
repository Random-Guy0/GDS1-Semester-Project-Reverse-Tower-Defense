using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaManager : MonoBehaviour
{
    public static ManaManager instance;
    public TMP_Text currentManaCount;

    public int currentMana;
    public int maxMana;
    public int minMana;

    private void Awake()
    {
        maxMana = 100;
        minMana = 0;
        instance = this;
    }
    void Start()
    {
        currentManaCount.text = "Mana: " + currentMana.ToString();
    }

    public int getMana() 
    {
        return currentMana; 
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
    }

    public bool ableToCost(int cost)
    {
        return currentMana - cost >= minMana;
    }
}
