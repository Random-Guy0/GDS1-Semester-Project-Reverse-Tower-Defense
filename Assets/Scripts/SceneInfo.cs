using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfo : MonoBehaviour
{
    public int SceneIndex { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(this);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (!scene.name.Equals("WinScreen") || scene.name.Equals("LoseScreen"))
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            Destroy(gameObject);
        }
    }
}