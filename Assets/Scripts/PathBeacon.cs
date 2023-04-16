using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBeacon : MonoBehaviour
{
    [SerializeField] private int pathCost;
    [SerializeField] private PathManager pathManager;
    [SerializeField] private ManaManager manaManager;
    
    void Update()
    {
        pathManager.SetSelectedTile(transform.position);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlacePath();
        }
    }

    public void PlacePath()
    {
        if (manaManager.ableToCost(pathCost) && pathManager.PlacePath(transform.position))
        {
            manaManager.costMana(pathCost);
        }
    }
}
