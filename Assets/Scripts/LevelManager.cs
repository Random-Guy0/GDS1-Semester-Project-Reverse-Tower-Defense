using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public void SelectLevel1Scene()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void SelectLevel2Scene()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void SelectLevel3Scene()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void SelectLevel4Scene()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void SelectLevel5Scene()
    {
        SceneManager.LoadScene("Level 5");
    }
    public void SelectLevel6Scene()
    {
        SceneManager.LoadScene("Level 6");
    }
    public void SelectLevel7Scene()
    {
        SceneManager.LoadScene("Level 7");
    }
    public void SelectLevel8Scene()
    {
        SceneManager.LoadScene("Level 8");
    }
    public void SelectLevel9Scene()
    {
        SceneManager.LoadScene("Level 9");
    }
    public void SelectLevel10Scene()
    {
        SceneManager.LoadScene("Level 10");
    }

    public void LoadLevelCreator()
    {
        SceneManager.LoadScene("UserLevels");
    }
    
    public void SelectMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void TryAgain()
    {
        SceneInfo levelToTryAgain = FindObjectOfType<SceneInfo>();
        SceneManager.sceneLoaded -= levelToTryAgain.OnSceneLoad;
        int index = levelToTryAgain.SceneIndex;

        if (!levelToTryAgain.LevelName.Equals(string.Empty))
        {
            GameObject levelNameObject = new GameObject("Level Name");
            LevelName levelNameComponent = levelNameObject.AddComponent<LevelName>();
            levelNameComponent.SetLevelName(levelToTryAgain.LevelName);
            DontDestroyOnLoad(levelNameObject);
        }
        
        Destroy(levelToTryAgain.gameObject);
        SceneManager.LoadScene(index);
    }

    public void NextLevel()
    {
        SceneInfo nextLevel = FindObjectOfType<SceneInfo>();
        SceneManager.sceneLoaded -= nextLevel.OnSceneLoad;
        int index = nextLevel.SceneIndex + 1;
        Destroy(nextLevel.gameObject);

        SceneManager.LoadScene(index);
    }
}