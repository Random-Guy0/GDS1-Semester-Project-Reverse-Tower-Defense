using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSegment : MonoBehaviour
{
    public float h;
    public float g;
    public PathSegment cameFrom;
    private List<PathSegment> connectedPathSegments = new List<PathSegment>();

    public void AddConnectedPathSegment(PathSegment pathSegment)
    {
        connectedPathSegments.Add(pathSegment);
    }

    public void RemoveConnectedPathSegment(PathSegment pathSegment)
    {
        connectedPathSegments.Remove(pathSegment);
    }

    public PathSegment[] GetConnectedPathSegments()
    {
        return connectedPathSegments.ToArray();
    }

    public float F()
    {
        return g + h;
    }
}
