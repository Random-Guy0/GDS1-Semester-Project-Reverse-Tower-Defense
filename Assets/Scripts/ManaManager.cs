using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    public int currentMana;
    public int maxMana;
    public int minMana;

    private void Awake()
    {
        maxMana = 100;
        minMana = 0;
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
}
