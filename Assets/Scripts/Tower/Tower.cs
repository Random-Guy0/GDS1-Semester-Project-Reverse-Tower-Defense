using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float fireDelay = 1.0f;
    public GameObject projectile;
    public float movementDelay = 60f;

    private FieldOfView fov;
    private Transform shootPoint;
    private Transform curTarget; 
    // Start is called before the first frame update
    void Start()
    {
        shootPoint = transform.GetChild(0);
        fov = GetComponent<FieldOfView>();
        StartCoroutine("FindTargetsWithDelay");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TargetFirst()
    {
        if (fov.visibleTargets.Count > 0 && fov.visibleTargets[0] != null)
        {
            curTarget = fov.visibleTargets[0];
        }
    }
    public IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {          
            TargetFirst();           
            if (curTarget != null)
            {
                ShootVisableTarget();
                yield return new WaitForSeconds(fireDelay);
            }
            else
            {
                yield return new WaitForSeconds(.2f);
            }
        }
    }
    void ShootVisableTarget()
    {
        transform.LookAt(curTarget);
        Instantiate(projectile,shootPoint.position, shootPoint.rotation);
        curTarget = null;
    }
}
