using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaMonster : Monster
{
    private NavigationNode[] allNodes;

    private void Start()
    {
        pathManager = FindObjectOfType<PathManager>();
        pathToFollow = new List<Vector3>();
        lastIndex = pathManager.GetPathSegmentIndex(pathManager.GetStart());
        
        AttachedNavigationNode[] attachedNodes = FindObjectsOfType<AttachedNavigationNode>();
        allNodes = new NavigationNode[pathManager.GetGrid().Length];

        for (int i = 0; i < attachedNodes.Length; i++)
        {
            int index = pathManager.GetIndexFromPosition(attachedNodes[i].navigationNode.position);
            allNodes[index] = attachedNodes[i].navigationNode;
        }
        
        GeneratePath();
    }

    protected override void ReachTarget()
    {
        lastIndex = pathManager.GetIndexFromPosition(pathToFollow[0]);
        pathToFollow.RemoveAt(0);
        GeneratePath();
    }

    protected override NavigationNode GetStart()
    {
        return allNodes[lastIndex];
    }

    protected override NavigationNode GetEnd()
    {
        ManaCollection[] allMana = FindObjectsOfType<ManaCollection>();
        if (allMana.Length > 0)
        {
            ManaCollection closest = allMana[0];
            for (int i = 1; i < allMana.Length; i++)
            {
                if (Vector3.Distance(transform.position, closest.transform.position) >
                    Vector3.Distance(transform.position, allMana[i].transform.position))
                {
                    closest = allMana[i];
                }
            }

            int index = pathManager.GetIndexFromPosition(closest.transform.position);
            return allNodes[index];
        }
        else
        {
            return null;
        }
    }

    protected override NavigationNode[] GetAllNodes()
    {
        return allNodes;
    }

    protected override NavigationNode[] GetConnectedNodes(NavigationNode current)
    {
        Vector3[] connectedPos = pathManager.ValidMovePositions(current.position);
        NavigationNode[] connectedNodes = new NavigationNode[connectedPos.Length];
        for (int i = 0; i < connectedPos.Length; i++)
        {
            int index = pathManager.GetIndexFromPosition(connectedPos[i]);
            connectedNodes[i] = allNodes[index];
        }

        return connectedNodes;
    }

    protected override void Update()
    {
        if (pathToFollow.Count == 0)
        {
            GeneratePath();
        }
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mana"))
        {
            other.GetComponent<ManaCollection>().Collect();
        }
    }
}
