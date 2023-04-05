using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    // Runs FindVisableTargets every x seconds
    // increasing the delay reduces the accuracy but also increases performace
    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    public IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisableTargets();
        }
    }
    // Finds all targets in agent's field of view
    void FindVisableTargets()
    {
        // Clear old targets
        visibleTargets.Clear();
        // Collect targets in radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //Debug.Log(targetsInViewRadius[i].name + "Close");
            // Get direction from target to this agent
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // Check if target is in field of view
            if (Vector3.Angle(transform.forward,dirToTarget) < viewAngle / 2)
            {
                //Debug.Log(targetsInViewRadius[i].name + "Detected");
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                // Raycast to see if target is obstructed
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget,obstacleMask))
                {
                    //Debug.Log(targetsInViewRadius[i].name + "Seen");
                    // Add targets
                    visibleTargets.Add(target);
                }
            }
        }
    }
    // Get the transform of a target with a given tag
    public Transform GetTargetFromTag(string tag)
    {
        foreach (Transform target in visibleTargets)
        {
            if (target.CompareTag(tag))
            {
                return target;
            }
        }
        return null;
    }
    // Used get the direction given the angle
    // refferenced by the FOV editor script to draw the edge of the agents field of view
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
