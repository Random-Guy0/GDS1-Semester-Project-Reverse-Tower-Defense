using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfo : MonoBehaviour
{
    public int SceneIndex { get; private set; }
    
    public string LevelName { get; private set; }

    private void Awake()
    {
        LevelName nameObject = FindObjectOfType<LevelName>();
        if (nameObject != null)
        {
            LevelName = nameObject.GetLevelName();
        }
        else
        {
            LevelName = String.Empty;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (!(scene.name.Equals("WinScreen") || scene.name.Equals("LoseScreen")))
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            Destroy(gameObject);
        }

        if (scene.name.Equals("WinScreen"))
        {
            GameObject.Find("Next Level").SetActive(false);
            GameObject mainMenuButton = GameObject.Find("Main Menu");
            Vector3 position = mainMenuButton.transform.position;
            position.x = 0;
            mainMenuButton.transform.position = position;
        }
    }
}