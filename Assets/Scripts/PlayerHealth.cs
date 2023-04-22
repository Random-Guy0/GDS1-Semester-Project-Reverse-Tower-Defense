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
        //maxHealth = 100;
        if(maxHealth==0)
        {
            maxHealth = 100;
        }
        health = maxHealth;
        healthBar.SetMaxHP(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (gameObject.CompareTag("Monster"))
            {
                FindObjectOfType<MonsterManager>().MonsterDeath(GetComponent<Monster>());
                Destroy(gameObject);
            }
            if (gameObject.CompareTag("Player"))
            {
                Application.Quit();
            }
        }
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
