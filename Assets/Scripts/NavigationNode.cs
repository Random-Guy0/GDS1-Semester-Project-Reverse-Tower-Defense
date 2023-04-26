using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationNode
{
    public Vector3 position;
    
    public float h;
    public float g;
    public NavigationNode cameFrom;
    public List<NavigationNode> connectedNodes;

    public NavigationNode(Vector3 position)
    {
        this.position = position;
        connectedNodes = new List<NavigationNode>();
    }

    public NavigationNode(Vector3 position, List<NavigationNode> connectedNodes)
    {
        this.position = position;
        this.connectedNodes = connectedNodes;
    }
    
    public float F()
    {
        return g + h;
    }
}
