using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public HPBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        healthBar.SetMaxHP(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageTake(int damage)
    {
        health -= damage;
        healthBar.SetHP(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.CompareTag("Attack"))
        {
            //DamagerTake(damage)
        }*/
    }
}
