using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winText;
    [SerializeField] private int levelHealth;

    public void TakeDamage()
    {
        levelHealth--;
        if (levelHealth <= 0)
        {
            winText.SetActive(true);
            Invoke("Close", 5.0f);
        }
    }

    private void Close()
    {
        Application.Quit();
    }
}
