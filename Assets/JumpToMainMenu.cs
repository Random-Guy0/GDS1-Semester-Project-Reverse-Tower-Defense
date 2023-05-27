using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToMainMenu : MonoBehaviour
{
    public float totalTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("toMainMenu", totalTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
