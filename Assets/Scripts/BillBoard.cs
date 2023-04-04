using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cameraP;

    private void LateUpdate()
    {
        transform.LookAt(cameraP.position + cameraP.forward);
    }
}
