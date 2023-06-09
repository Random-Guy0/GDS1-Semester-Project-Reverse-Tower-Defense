using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Tooltip("The time it takes (in seconds) to attack again")]
    public float fireDelay = 1.0f;
    [Tooltip("The time it takes (in seconds) to ready an attack while aiming")]
    public float shootStartUp = 0f;
    [Tooltip("The time it takes (in seconds) to ready an attack WITHOUT aiming just for the player")]
    public float shootPlayerDelay = 0.1f;
    public bool displayAimLaser = false;
    public GameObject projectile;
    public float movementDelay = 60f;
    public bool immovable = false;
    public float spawnTime = 0f;
    public GameObject prestigeClass;
    public float prestigeTime = 120f;
    public Animator animator;
    public GameObject warningSign;
    public float warningSignTime = 30f;
    public ParticleSystem wakeParticles;
    public GameObject warningCanvas;

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
    private float fireDelayMod = 1;
    private bool shooting;
    private LineRenderer lr;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {     
        pm = GameObject.Find("Path Manager").GetComponent<PathManager>();
        Model = transform.GetChild(1);
        anim = Model.gameObject.GetComponent<Animator>();
        shootPoint = transform.GetChild(0);
        lr = shootPoint.GetComponent<LineRenderer>();
        fov = GetComponent<FieldOfView>();
        if (spawnTime > 0)
        {
            TowerActive(false);
            StartCoroutine(SpawnWithDelay(spawnTime));
        }
        if (prestigeClass != null && prestigeTime > 0)
        {
            StartCoroutine(PromotionWithDelay(prestigeTime));
        }
        StartCoroutine(FindTargetsWithDelay());
        if (!immovable)
        {
            StartCoroutine(MoveWithDelay());
        } 
    }
    public IEnumerator PromotionWithDelay(float duration)
    {
        // wait for tower to activate
        yield return new WaitUntil(new System.Func<bool>(() => GetState()));
        // check if the warning sign should activate immidiatly 
        if (duration < warningSignTime)
        {
            warningCanvas.SetActive(true);
            yield return new WaitForSeconds(duration);
        }
        else
        {
            // Wait to spawn warning sign
            yield return new WaitForSeconds(duration - warningSignTime);
            warningCanvas.SetActive(true);
            yield return new WaitForSeconds(warningSignTime);
        }
        // When time is up, destroy this tower and spawn new tower
        if (gameObject != null)
        {
            warningCanvas.SetActive(false);
            GameObject tmp = Instantiate(prestigeClass,transform.position, transform.rotation);
            tmp.GetComponent<Tower>().wakeParticles.Play();
            DestroyTower();
        }
    }
    private void Update()
    {
        if(shooting)
        {
            transform.LookAt(curTarget);
            //Debug.Log("curTarget velocity y: " + curTarget.GetComponent<Rigidbody>().velocity.y);
            if (displayAimLaser)
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, curTarget.position);
            }
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
    public void DestroyTower()
    {
        Destroy(gameObject);
    }
    public bool GetState()
    {
        return state;
    }
    public float GetFireDelayMod()
    {
        return fireDelayMod;
    }
    public void SetFireDelayMod(float newFireDelay)
    {
        fireDelayMod = newFireDelay;
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
            else
            {
                curTarget = fov.visibleTargets[0];
            }           
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
                if (shootStartUp > 0)
                {
                    shooting = true;
                    if(displayAimLaser)
                    {
                        lr.enabled = true;
                    }
                    yield return new WaitForSeconds(shootStartUp);
                    lr.enabled = false;
                    shooting = false;
                    curTarget = null;
                    TargetFirstEnemy();
                }
                if (curTarget != null)
                {
                    StartCoroutine("ShootVisableTarget");
                    //ShootVisableTarget();
                    yield return new WaitForSeconds(fireDelay * fireDelayMod);
                }
            }
            else if (!firing)
            {
                yield return new WaitUntil(new System.Func<bool>(() => GetState()));
                yield return new WaitForSeconds(fireDelay * fireDelayMod);
            }
            else
            {
                yield return new WaitForSeconds(.2f);
            }
        }
    }
    IEnumerator ShootVisableTarget()
    {
        transform.LookAt(curTarget);
        if (curTarget.CompareTag("Player"))
        {
            yield return new WaitForSeconds(shootPlayerDelay);
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
        if (projectile.name == "Hit")
        {
            anim.SetTrigger("KnightAttack");
        }
        else if (projectile.name == "BIG Hit")
        {
            anim.SetTrigger("Hylian");
        }
        else if (projectile.name == "FireBall")
        {
            anim.SetTrigger("Magic");
        }
        else if (projectile.name != "Balista Bolt")
        {
            anim.SetTrigger("Shoot");
        }
        Instantiate(projectile,shootPoint.position, shootPoint.rotation);
        curTarget = null;
    }
}
