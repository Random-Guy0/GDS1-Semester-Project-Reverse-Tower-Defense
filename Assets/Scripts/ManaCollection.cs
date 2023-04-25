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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.CollectMana(transform.position);
            other.gameObject.GetComponent<Movement>().animator.SetBool("IsPickUp", false);
            Destroy(gameObject);
        }
    }
}
