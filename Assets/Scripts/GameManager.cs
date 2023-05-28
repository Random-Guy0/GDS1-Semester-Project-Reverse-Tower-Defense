using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelHealthText;
    [SerializeField] private int levelHealth;
    [SerializeField] private Animator PlayerAni;
    [SerializeField] GameObject originalGameObject;
    
    public bool HasWon { get; private set; }

    private void Start()
    {
        SetHealthText();
        originalGameObject = GameObject.Find("Player");
        PlayerAni = originalGameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();
    }

    public void SetGateHealth(int levelHealth)
    {
        this.levelHealth = levelHealth;
    }

    public void TakeDamage(int damage)
    {
        levelHealth -= damage;
        if (levelHealth < 0)
        {
            levelHealth = 0;
        }
        SetHealthText();
        if (levelHealth <= 0)
        {
            HasWon = true;
            originalGameObject.GetComponent<Movement>().playerSpeed = 0;
            Invoke("loadWin", 3.5f);
        }
    }

    private void loadWin()
    {
        SceneManager.LoadScene("WinScreen");
    }

    private void SetHealthText()
    {
        levelHealthText.SetText(levelHealth.ToString());
    }

    public int getLevelHealth()
    {
        return levelHealth;
    }
}
