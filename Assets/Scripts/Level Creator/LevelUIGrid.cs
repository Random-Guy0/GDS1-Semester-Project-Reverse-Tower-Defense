using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIGrid : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text levelNameText;
    private DeleteLevel deleteLevelDialogue;

    private string levelName;

    private void Start()
    {
        deleteLevelDialogue = FindObjectOfType<DeleteLevel>(true);
    }

    public void SetLevel(string levelName)
    {
        this.levelName = levelName;
        levelNameText.SetText(levelName);

        string path = Application.persistentDataPath + "/User Levels/" + levelName + "/icon.png";

        if (System.IO.File.Exists(path))
        {
            Byte[] iconFile = System.IO.File.ReadAllBytes(path);
            Texture2D tex2d = new Texture2D(512, 512);
            tex2d.LoadImage(iconFile);
            icon.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.one * 0.5f);
        }

    }
    
    public void PlayLevel()
    {
        GameObject levelNameObject = new GameObject("Level Name");
        LevelName levelNameComponent = levelNameObject.AddComponent<LevelName>();
        levelNameComponent.SetLevelName(levelName);
        DontDestroyOnLoad(levelNameObject);
        SceneManager.LoadScene("UserLevelPlayer");
    }

    public void EditLevel()
    {
        GameObject levelNameObject = new GameObject("Level Name");
        LevelName levelNameComponent = levelNameObject.AddComponent<LevelName>();
        levelNameComponent.SetLevelName(levelName);
        DontDestroyOnLoad(levelNameObject);
        SceneManager.LoadScene("LevelCreator");
    }

    public void DeleteLevel()
    {
        deleteLevelDialogue.gameObject.SetActive(true);
        deleteLevelDialogue.SetLevel(levelName, gameObject);
    }
}
