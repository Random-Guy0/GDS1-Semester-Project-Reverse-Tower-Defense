using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    public int damage = 2;
    public float lifeTime = 1;
    public int pericing = 0;
    public GameObject HitEffect;
    [Tooltip("When Projectile hits a target, the next phase will be instatiated. This field is NOT maditory")]
    public GameObject NextPhase;
    public LayerMask NoHitLayerMask;

    private bool shouldBeDead = false;
    private Rigidbody rb;
    private List<GameObject> hitTargets = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        StartCoroutine("LifeSpan", lifeTime);        
    }
    public IEnumerator LifeSpan(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (gameObject != null)
        {
            ActivateOnDestroy();
        }
        
    }
    void ActivateOnHit()
    {
        if (HitEffect != null)
        {
            Instantiate(HitEffect, transform.position, transform.rotation);
        }
    }
    void ActivateOnDestroy()
    {
        if (NextPhase != null)
        {
            Instantiate(NextPhase,transform.position,transform.rotation);           
        }
        shouldBeDead = true;
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (shouldBeDead)
        {
            return;
        }
        //Debug.Log((NoHitLayerMask.value >> collision.gameObject.layer) % 2);
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Monster"))
        {
            if (!hitTargets.Contains(collision.transform.gameObject))
            {
                hitTargets.Add(collision.transform.gameObject);
                collision.transform.GetComponent<PlayerHealth>().DamageTake(damage);
                if (HitEffect != null)
                {
                    ActivateOnHit();
                }
                if (pericing <= 0)
                {
                    ActivateOnDestroy();
                }
                else
                {
                    pericing--;
                }
            }
        }
        else if ((NoHitLayerMask.value >> collision.gameObject.layer)%2 == 1)
        {

        }
        else
        {
            ActivateOnDestroy();
        }       
    }
}
