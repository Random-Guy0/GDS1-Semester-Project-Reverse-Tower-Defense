using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollection : MonoBehaviour
{
    public GameObject ManaManager;
    public ManaManager manager;

    private void Start()
    {
        ManaManager = GameObject.Find("ManaManager");
        manager = ManaManager.GetComponent<ManaManager>();
    }
    
    public void Collect()
    {
        manager.CollectMana(transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collect();
			other.gameObject.GetComponent<Movement>().animator.SetBool("IsPickUp", false);
        }
    }
}
