using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSegment : MonoBehaviour
{
    private List<PathSegment> connectedPathSegments;

    public void AddConnectedPathSegment(PathSegment pathSegment)
    {
        connectedPathSegments.Add(pathSegment);
    }

    public PathSegment[] GetConnectedPathSegments()
    {
        return connectedPathSegments.ToArray();
    }
}
