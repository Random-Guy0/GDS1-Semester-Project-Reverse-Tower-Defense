using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBeacon : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //place the beacon to make path
            Debug.Log("Put down the beacon");
        }
    }
}
