using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject currentCamera;
    public GameObject[] cameras;
    public int currentNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentCamera = cameras[currentNum];
        gameObject.transform.position = currentCamera.transform.position;
        gameObject.transform.rotation = currentCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentNum <3)
            {
                currentNum++;
            }else if(currentNum == 3)
            {
                currentNum = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentNum > 0)
            {
                currentNum--;
            }
            else if (currentNum == 0)
            {
                currentNum = 3;
            }
        }
        setCamera();
    }

    void setCamera()
    {
        currentCamera = cameras[currentNum];
        transform.position = Vector3.MoveTowards(transform.position, currentCamera.transform.position, 50f*Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentCamera.transform.rotation, 8f * Time.deltaTime);
    }

    public string getCurrentCamera()
    {
        return currentCamera.name;
    }


}
