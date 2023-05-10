using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    [SerializeField] private int damage = 1;
    private Animator animator;

    protected PathManager pathManager;
    protected List<Vector3> pathToFollow;
    protected int lastIndex;

    private void Start()
    {
        // Get the sub GameObject within the current GameObject
        animator = GetComponent<Animator>();
        pathManager = FindObjectOfType<PathManager>();
        pathToFollow = new List<Vector3>();
        lastIndex = pathManager.GetPathSegmentIndex(pathManager.GetStart());

        GeneratePath();
    }

    protected virtual void Update()
    {
        if(pathToFollow.Count > 0)
        {
            Vector3 targetPos = pathToFollow[0];
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
            Vector3 position = transform.position;
            Vector3 moveDirection = (targetPos - position).normalized;
            position += speed * Time.deltaTime * moveDirection;
            position.y = pathManager.MeshHeight(pathManager.GetIndexFromPosition(position)) - 0.3f;
            transform.position = position;

            if (Vector3.Distance(transform.position, targetPos) < 0.05)
            {
                ReachTarget();
            }
        }
    }

    protected virtual void ReachTarget()
    {
        if (pathToFollow[0].Equals(pathManager.GetEnd().navigationNode.position))
        {
            FindObjectOfType<GameManager>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            lastIndex = pathManager.GetIndexFromPosition(pathToFollow[0]);
            pathToFollow.RemoveAt(0);
        }
    }

    protected virtual NavigationNode GetStart()
    {
        PathSegment[] pathSegments = pathManager.GetPathSegments();
        PathSegment start = pathSegments[lastIndex];
        if (start != null)
        {
            return start.navigationNode;
        }
        else
        {
            return null;
        }
    }

    protected virtual NavigationNode GetEnd()
    {
        return pathManager.GetEnd().navigationNode;
    }

    protected virtual NavigationNode[] GetAllNodes()
    {
        PathSegment[] pathSegments = pathManager.GetPathSegments();
        List<NavigationNode> allNodes = new List<NavigationNode>();
        foreach (PathSegment pathSegment in pathSegments)
        {
            if (pathSegment != null)
            {
                allNodes.Add(pathSegment.navigationNode);
            }
        }

        return allNodes.ToArray();
    }

    protected virtual NavigationNode[] GetConnectedNodes(NavigationNode current)
    {
        return current.connectedNodes.ToArray();
    }

    public void GeneratePath()
    {
        NavigationNode[] allNodes = GetAllNodes();
        NavigationNode start = GetStart();
        if (start == null)
        {
            return;
        }
        NavigationNode end = GetEnd();
        if (end == null)
        {
            return;
        }

        List<NavigationNode> openSet = new List<NavigationNode>();
        openSet.Add(start);
        
        foreach(NavigationNode node in allNodes)
        {
            if (node != null)
            {
                node.g = float.PositiveInfinity;
            }
        }

        start.g = 0;
        start.h = Vector3.Distance(start.position, end.position);

        NavigationNode closestToEnd = openSet[0];
        while (openSet.Count > 0)
        {
            NavigationNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F() < current.F())
                {
                    current = openSet[i];
                }
            }

            if (Vector3.Distance(current.position, end.position) <=
                Vector3.Distance(closestToEnd.position, end.position))
            {
                closestToEnd = current;
            }

            openSet.Remove(current);

            if (current.Equals(end))
            {
                pathToFollow = ReconstructPath(start, end);
                return;
            }

            NavigationNode[] connectedNodes = GetConnectedNodes(current);
            
            foreach (NavigationNode connectedNode in connectedNodes)
            {
                float tentativeGScore = current.g +
                                        Vector3.Distance(current.position, connectedNode.position);

                if (tentativeGScore < connectedNode.g)
                {
                    connectedNode.cameFrom = current;
                    connectedNode.g = tentativeGScore;
                    connectedNode.h = Vector3.Distance(connectedNode.position, end.position);
                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        pathToFollow = ReconstructPath(start, closestToEnd);
    }

    private List<Vector3> ReconstructPath(NavigationNode start, NavigationNode end)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(end.position);
        NavigationNode current = end;
        while (!current.Equals(start))
        {
            current = current.cameFrom;
            path.Add(current.position);
        }
        
        path.Reverse();
        path.Remove(start.position);

        return path;
    }

    public float SetAnimation(string animationName)
    {
        if (animationName == "Death")
        {
           animator.SetBool("Death", true);
           return animator.GetCurrentAnimatorStateInfo(0).length;
        } else if (animationName == "Hit")
        {
            animator.SetTrigger("Hit");
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }

        return 0;


    }

}