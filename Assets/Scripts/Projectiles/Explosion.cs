using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Vector3 possOffset;
    public int damage = 5;
    [Tooltip("How many entites can be hit by an explosion. Set to 0 for no limit")]
    public int maxTargets = 0;
    public float radius = 3;
    public float delay = 0.1f;
    public LayerMask mask;


    private int curTargets = 0;
    private RaycastHit[] entities;
    private List<GameObject> hitTargets = new List<GameObject>();
    private Vector3 trueOffset;
    // Start is called before the first frame update
    void Start()
    {
        trueOffset = Quaternion.Euler(transform.rotation.eulerAngles) * possOffset;
        StartCoroutine("Detonate");
    }
    // Update is called once per frame
    IEnumerator Detonate()
    {
        yield return new WaitForSeconds(delay);
        entities = Physics.SphereCastAll(transform.position + trueOffset, radius, Vector3.forward, 0, mask);
        foreach (RaycastHit entity in entities)
        {
            if (entity.transform.CompareTag("Player") || entity.transform.CompareTag("Monster"))
            {
                if (!hitTargets.Contains(entity.transform.gameObject))
                {
                    hitTargets.Add(entity.transform.gameObject);
                    entity.transform.GetComponent<PlayerHealth>().DamageTake(damage);
                    curTargets++;
                    if (maxTargets != 0 && curTargets >= maxTargets)
                    {
                        break;
                    }
                }

            }
        }
        //hitTargets.Clear();
    }
    private void OnDrawGizmos()
    {
        trueOffset = Quaternion.Euler(transform.rotation.eulerAngles) * possOffset;
        Gizmos.DrawWireSphere(transform.position + trueOffset, radius);
    }
}
