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
    }
}