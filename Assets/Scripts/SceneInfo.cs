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

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (!scene.name.Equals("LoseScreen"))
        {
            Destroy(gameObject);
        }
    }
}