using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float fireDelay = 1.0f;
    public float shootStartUp = 0f;
    public GameObject projectile;
    public float movementDelay = 60f;
    public bool immovable = false;
    public float spawnTime = 0f;
    public GameObject presitgeClass;
    public float prestigeTime = 120f;
    public Animator animator;
    public GameObject warningSign;
    public float warningSignTime = 30f;
    public ParticleSystem wakeParticles;

    private Transform Model;
    private FieldOfView fov;
    private Transform shootPoint;
    private Transform curTarget;
    private PathManager pm;
    private bool firing = true;
    private bool moving = true;
    private bool state = true;
    private PathSegment[] ps;
    private GridTile TilePos;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("Path Manager").GetComponent<PathManager>();
        Model = transform.GetChild(1);      
        shootPoint = transform.GetChild(0);
        fov = GetComponent<FieldOfView>();
        if (spawnTime > 0)
        {
            TowerActive(false);
            StartCoroutine("SpawnWithDelay", spawnTime);
        }
        StartCoroutine("FindTargetsWithDelay");
        if (!immovable)
        {
            StartCoroutine("MoveWithDelay");
        }   
        
    }
    public IEnumerator SpawnWithDelay(float duration)
    {
        if (duration < warningSignTime)
        {
            warningSign.SetActive(true);
            yield return new WaitForSeconds(duration);
        }
        else
        {
            yield return new WaitForSeconds(duration-warningSignTime);
            warningSign.SetActive(true);
            yield return new WaitForSeconds(warningSignTime);
        }
        if (gameObject != null)
        {
            warningSign.SetActive(false);
            wakeParticles.Play();
            TowerActive(true);
        }
    }
    public void TowerActive(bool _state)
    {
        Model.gameObject.SetActive(_state);
        firing = _state;
        state = _state;
    }
    public bool GetState()
    {
        return state;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator MoveWithDelay()
    {
        while (true)
        {
            // Collect Path tiles
            //ps = pm.GetPathSegments();
            // Collect nearby walkable tiles
            //pm.GetGridPoint(transform.position);
            // Calculate which tiles cover the most path considering range
            // Set that tile to your closest tile.
            if (moving && curTarget != null)
            {
                // move to spot
                yield return new WaitForSeconds(movementDelay);
                
            }
            else if (!moving)
            {
                yield return new WaitUntil(new System.Func<bool>(() => GetState()));
                yield return new WaitForSeconds(movementDelay);
            }
            else
            {
                yield return new WaitForSeconds(.2f);
            }
        }
    }
    void TargetFirst()
    {
        if (fov.visibleTargets.Count > 0 && fov.visibleTargets[0] != null)
        {
            curTarget = fov.visibleTargets[0];
        }
    }
    void TargetFirstEnemy()
    {
        if (fov.visibleTargets.Count > 0 && fov.visibleTargets[0] != null)
        {
            if (fov.visibleTargets[0].CompareTag("Player"))
            {
                if (fov.visibleTargets.Count > 1 && fov.visibleTargets[1] != null)
                {
                    curTarget = fov.visibleTargets[1];
                }
                else
                {
                    curTarget = fov.visibleTargets[0];
                }
            }
            curTarget = fov.visibleTargets[0];
        }
    }
    public IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {
            TargetFirstEnemy();
            if (firing && curTarget != null)
            {
                transform.LookAt(curTarget);
                yield return new WaitForSeconds(shootStartUp);
                curTarget = null;
                TargetFirstEnemy();
                if (curTarget != null)
                {
                    ShootVisableTarget();
                    yield return new WaitForSeconds(fireDelay);
                }
            }
            else if (!firing)
            {
                yield return new WaitUntil(new System.Func<bool>(() => GetState()));
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
