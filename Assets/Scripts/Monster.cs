using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    
    [SerializeField] private float speed;

    private PathManager path;
    private PathSegment current;
    private PathSegment target = null;
    private PathSegment previous;

    private void Start()
    {
        path = FindObjectOfType<PathManager>();
        current = path.GetStart();
        previous = current;
    }

    private void Update()
    {
        if (target == null)
        {
            SetNextTarget();
        }
        else
        {
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            transform.position =
                Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (transform.position.Equals(targetPos))
            {
                previous = current;
                current = target;
                target = null;
            }
        }
    }

    private void SetNextTarget()
    {
        PathSegment[] options = current.GetConnectedPathSegments();

        if (options.Length > 0)
        {
            PathSegment selectedOption = options[0];
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i] != previous &&
                    Vector3.Distance(options[i].transform.position, path.GetEnd().transform.position) <
                    Vector3.Distance(selectedOption.transform.position, path.GetEnd().transform.position))
                {
                    selectedOption = options[i];
                }
            }
        }
    }
}
