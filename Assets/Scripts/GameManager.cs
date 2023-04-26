using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
            SceneManager.LoadScene("WinScreen");
        }
    }

    private void SetHealthText()
    {
        levelHealthText.SetText(levelHealth.ToString());
    }
}
