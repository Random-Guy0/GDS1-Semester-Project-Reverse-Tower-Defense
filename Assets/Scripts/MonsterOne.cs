
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterOne : MonoBehaviour
{
    public float moveSpeed = 5f;
    private int currentWaypointLength = 0;
    public int currentWaypointIndex = 0;
    private PathManager pathManager;
    public double test;

    private void Start()
    {
        pathManager = FindObjectOfType<PathManager>();
        pathManager.waypoints.Add(transform.position);
    }

    private void Update()
    {
        currentWaypointLength = pathManager.waypoints.Count;
        if (currentWaypointIndex < currentWaypointLength)
        {
            Vector3 targetWaypoint = pathManager.waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, targetWaypoint) < 1f)
            {
                if(currentWaypointIndex+1< currentWaypointLength)
                {
                    if (Vector3.Distance(pathManager.waypoints[currentWaypointIndex], pathManager.waypoints[currentWaypointIndex+1]) <= 2.5f)
                    {
                        currentWaypointIndex++;
                    }
                    else
                    {
                        Vector3 removedItem = pathManager.waypoints[currentWaypointIndex+1];
                        pathManager.waypoints.RemoveAt(currentWaypointIndex+1);
                        pathManager.waypoints.Add(removedItem);
                    }
                }
            }
            Vector3 moveDirection = (targetWaypoint - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            if(currentWaypointLength>1)
               {
                test = Vector3.Distance(pathManager.waypoints[0], pathManager.waypoints[1]);
                }     

            

           
        }
        else
        {
            // We've reached the end of the path, do something here
        }
    }
}