using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterOne : MonoBehaviour
{

    public GameObject Target;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Target.gameObject.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
