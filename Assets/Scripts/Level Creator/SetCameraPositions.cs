using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraPositions : MonoBehaviour
{
    [SerializeField] private GameObject angle1;
    [SerializeField] private GameObject angle2;
    [SerializeField] private GameObject angle3;
    [SerializeField] private GameObject angle4;

    private void Start()
    {
        PathManager pathManager = FindObjectOfType<PathManager>();
        int levelWidth = pathManager.GetLevelWidth();
        int levelDepth = pathManager.GetLevelDepth();
        float gridSize = pathManager.GetGridSize();

        float h = Mathf.Sqrt(levelWidth * levelWidth + levelDepth * levelDepth);
        if (h < 10f)
        {
            h = 10;
        }

        Vector3 angle1Pos = angle1.transform.position;
        angle1Pos.x = levelWidth * gridSize * 0.5f;
        angle1Pos.y = h;
        angle1.transform.position = angle1Pos;
        
        Vector3 angle2Pos = angle2.transform.position;
        angle2Pos.x = levelWidth * gridSize + 5f;
        angle2Pos.y = h;
        angle2Pos.z = levelDepth * gridSize * 0.5f;
        angle2.transform.position = angle2Pos;
        
        Vector3 angle3Pos = angle3.transform.position;
        angle3Pos.x = levelWidth * gridSize * 0.5f;
        angle3Pos.y = h;
        angle3Pos.z = levelDepth * gridSize + 5f;
        angle3.transform.position = angle3Pos;
        
        Vector3 angle4Pos = angle4.transform.position;
        angle4Pos.y = h;
        angle4Pos.z = levelDepth * gridSize * 0.5f;
        angle4.transform.position = angle4Pos;
    }
}
