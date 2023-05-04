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
        if (scene.buildIndex != 7)
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            Destroy(gameObject);
        }
    }
}