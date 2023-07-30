using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelName : MonoBehaviour
{
    private string levelName;

    public void SetLevelName(string levelName)
    {
        this.levelName = levelName;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        if (scene.name.Equals("LevelCreator"))
        {
            FindObjectOfType<UserLevelCreator>().SetLevelName(levelName);
        }
        Destroy(gameObject);
    }
}
