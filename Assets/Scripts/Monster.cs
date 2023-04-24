using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed;

    private PathManager pathManager;
    private List<PathSegment> pathToFollow;
    private int lastIndex;

    private void Start()
    {
        pathManager = FindObjectOfType<PathManager>();
        pathToFollow = new List<PathSegment>();
        lastIndex = pathManager.GetPathSegmentIndex(pathManager.GetStart());

        GeneratePath();
    }

    private void Update()
    {
        if(pathToFollow.Count > 0)
        {
            Vector3 targetPos = pathToFollow[0].transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
            Vector3 moveDirection = (targetPos - transform.position).normalized;
            transform.position += speed * Time.deltaTime * moveDirection;

            if (Vector3.Distance(transform.position, targetPos) < 0.05)
            {
                if (pathToFollow[0].Equals(pathManager.GetEnd()))
                {
                    FindObjectOfType<GameManager>().TakeDamage();
                    Destroy(gameObject);
                }
                else
                {
                    lastIndex = pathManager.GetPathSegmentIndex(pathToFollow[0]);
                    pathToFollow.RemoveAt(0);
                }
            }
        }
    }

    public void GeneratePath()
    {
        PathSegment[] pathSegments = pathManager.GetPathSegments();
        PathSegment start = pathSegments[lastIndex];
        if (start == null)
        {
            return;
        }
        PathSegment end = pathManager.GetEnd();

        List<PathSegment> openSet = new List<PathSegment>();
        openSet.Add(start);
        
        foreach(PathSegment pathSegment in pathSegments)
        {
            if (pathSegment != null)
            {
                pathSegment.g = float.PositiveInfinity;
            }
        }

        start.g = 0;
        start.h = Vector3.Distance(start.transform.position, end.transform.position);

        PathSegment closestToEnd = openSet[0];
        while (openSet.Count > 0)
        {
            PathSegment current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F() < current.F())
                {
                    current = openSet[i];
                }
            }

            if (Vector3.Distance(current.transform.position, end.transform.position) <=
                Vector3.Distance(closestToEnd.transform.position, end.transform.position))
            {
                closestToEnd = current;
            }

            openSet.Remove(current);

            if (current.Equals(end))
            {
                pathToFollow = ReconstructPath(start, end);
                return;
            }
            
            foreach (PathSegment connectedPath in current.GetConnectedPathSegments())
            {
                float tentativeGScore = current.g +
                                        Vector3.Distance(current.transform.position, connectedPath.transform.position);

                if (tentativeGScore < connectedPath.g)
                {
                    connectedPath.cameFrom = current;
                    connectedPath.g = tentativeGScore;
                    connectedPath.h = Vector3.Distance(connectedPath.transform.position, end.transform.position);
                    if (!openSet.Contains(connectedPath))
                    {
                        openSet.Add(connectedPath);
                    }
                }
            }
        }

        pathToFollow = ReconstructPath(start, closestToEnd);
    }

    private List<PathSegment> ReconstructPath(PathSegment start, PathSegment end)
    {
        List<PathSegment> path = new List<PathSegment>();
        path.Add(end);
        PathSegment current = end;
        while (!current.Equals(start))
        {
            current = current.cameFrom;
            path.Add(current);
        }
        
        path.Reverse();
        path.Remove(start);

        return path;
    }
}