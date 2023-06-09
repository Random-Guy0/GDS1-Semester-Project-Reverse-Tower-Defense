using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSegment : MonoBehaviour
{
    public NavigationNode navigationNode;
    public List<PathSegment> connectedPathSegments = new List<PathSegment>();

    private void Awake()
    {
        List<NavigationNode> connectedNodes = new List<NavigationNode>();
        foreach (PathSegment pathSegment in connectedPathSegments)
        {
            connectedNodes.Add(pathSegment.navigationNode);
        }
        navigationNode = new NavigationNode(transform.position, connectedNodes);
    }

    public void AddConnectedPathSegment(PathSegment pathSegment)
    {
        connectedPathSegments.Add(pathSegment);
        navigationNode.connectedNodes.Add(pathSegment.navigationNode);
    }

    public void RemoveConnectedPathSegment(PathSegment pathSegment)
    {
        connectedPathSegments.Remove(pathSegment);
        navigationNode.connectedNodes.Remove(pathSegment.navigationNode);
    }

    public PathSegment[] GetConnectedPathSegments()
    {
        return connectedPathSegments.ToArray();
    }

    public void Clone(PathSegment other)
    {
        this.navigationNode = other.navigationNode;
        this.connectedPathSegments = other.connectedPathSegments;

        foreach (PathSegment connnectedPathSegment in connectedPathSegments)
        {
            connnectedPathSegment.connectedPathSegments.Remove(other);
            connnectedPathSegment.connectedPathSegments.Add(this);
        }
    }
}
