using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private float duration;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i < towers.Length; i++)
        {
            if (towers[i].name == "WizardTower")
            {
                towers[i].GetComponent<Tower>().fireDelay = 2;
            }
            else
            {
                towers[i].GetComponent<Tower>().fireDelay = 1;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
         if (other.gameObject.CompareTag("Tower"))
         {
            other.GetComponent<Tower>().fireDelay = 5;
         }
    }

}
