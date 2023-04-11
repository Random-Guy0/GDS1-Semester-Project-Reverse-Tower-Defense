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
    public void SelectMainMenu()
    {
        SceneManager.LoadScene("Main Menu");

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame

}