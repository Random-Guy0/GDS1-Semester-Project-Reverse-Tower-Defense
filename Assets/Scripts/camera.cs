using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class camera : MonoBehaviour
{
    public GameObject currentCamera;
    public GameObject[] cameras;
    public int currentNum = 0;
    public Transform oldCamera;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        currentCamera = cameras[currentNum];
        oldCamera = currentCamera.transform;
        startTime = Time.time;
        gameObject.transform.position = currentCamera.transform.position;
        gameObject.transform.rotation = currentCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            if(currentNum <3)
            {
                oldCamera = cameras[currentNum].transform;
                currentNum++;
            }else if(currentNum == 3)
            {
                oldCamera = cameras[currentNum].transform;
                currentNum = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (currentNum > 0)
            {
                oldCamera = cameras[currentNum].transform;
                currentNum--;
            }
            else if (currentNum == 0)
            {
                oldCamera = cameras[currentNum].transform;
                currentNum = 3;
            }
        }
        setCamera();
    }

    void setCamera()
    {
        currentCamera = cameras[currentNum];
        transform.position = Vector3.MoveTowards(transform.position, currentCamera.transform.position, 40f*Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentCamera.transform.rotation, 4f * Time.deltaTime);
    }

    public string getCurrentCamera()
    {
        return currentCamera.name;
    }


}
