using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class UsersLevelsScreen : MonoBehaviour
{
    [SerializeField] private GameObject CantOpenDialog;
    [SerializeField] private GameObject createLevelDialogue;
    [SerializeField] private TMP_InputField levelNameInput;
    private string userLevelsFolder;

    [SerializeField] private GameObject levelGridItemPrefab;
    [SerializeField] private Transform scrollViewContent;
    
    private void Start()
    {
        userLevelsFolder = Application.persistentDataPath + "/User Levels";
        if (!Directory.Exists(userLevelsFolder))
        {
            Directory.CreateDirectory(userLevelsFolder);
        }

        DirectoryInfo[] levels = new DirectoryInfo(userLevelsFolder).GetDirectories();
        foreach(DirectoryInfo level in levels)
        {
            GameObject levelItem = Instantiate(levelGridItemPrefab, scrollViewContent);
            levelItem.GetComponent<LevelUIGrid>().SetLevel(level.Name);
        }
    }

    public void OpenLevelsFolder()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        CantOpenDialog.SetActive(true);
        #else
        Application.OpenURL(userLevelsFolder);
        #endif
    }

    public void CloseDialog(GameObject dialog)
    {
        dialog.SetActive(false);
    }

    public void OpenCreateLevelDialogue()
    {
        createLevelDialogue.SetActive(true);
    }

    public void CreateLevel()
    {
        string levelName = levelNameInput.text;
        if (!levelName.Equals(String.Empty))
        {
            GameObject levelNameObject = new GameObject("Level Name");
            LevelName levelNameComponent = levelNameObject.AddComponent<LevelName>();
            levelNameComponent.SetLevelName(levelName);
            DontDestroyOnLoad(levelNameObject);
            SceneManager.LoadScene("LevelCreator");
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
