using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string levelHealthTextPrefix;
    [SerializeField] private TMP_Text levelHealthText;
    [SerializeField] private GameObject winText;
    [SerializeField] private int levelHealth;

    private void Start()
    {
        SetHealthText();
    }

    public void TakeDamage()
    {
        levelHealth--;
        SetHealthText();
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

    private void SetHealthText()
    {
        levelHealthText.SetText(levelHealthTextPrefix + ": " + levelHealth);
    }
}
