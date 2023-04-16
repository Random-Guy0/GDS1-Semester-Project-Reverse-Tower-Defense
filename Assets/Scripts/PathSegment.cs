using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSegment : MonoBehaviour
{
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
}
