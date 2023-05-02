using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject StartTips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        if (!StartTips.activeSelf)
        {
            Time.timeScale = 1;
        }
    }
}
