using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollection : MonoBehaviour
{
    public GameObject ManaManager;
    public ManaManager manager;
    [SerializeField] private GameObject Player;

    private void Awake()
    {
        ManaManager = GameObject.Find("ManaManager");
        manager = ManaManager.GetComponent<ManaManager>();
    }
    
    public void Collect()
    {
        Player.GetComponent<Movement>().animator.SetBool("IsPickUp", false);
        manager.CollectMana(transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = other.gameObject;
            Invoke("Collect", 0.2f);
        }
    }
}
