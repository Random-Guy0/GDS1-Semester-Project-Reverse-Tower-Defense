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