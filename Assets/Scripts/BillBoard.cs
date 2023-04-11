using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cameraP;

    private Quaternion originalRotation;

    private void Start()
    {
        cameraP = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.forward = cameraP.forward;
    }
}
