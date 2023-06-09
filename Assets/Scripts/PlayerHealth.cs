using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public HPBar healthBar;
    public AudioClip MonDeathSound;
    private bool isPlayed;
    public GameObject PopupDamager;

    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = 100;
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

                // Start the coroutine that waits for the animation to finish
                StartCoroutine(WaitForDeathAnimation("Death"));

                
            }
            if (gameObject.CompareTag("Player") && !FindObjectOfType<GameManager>().HasWon)
            {
                gameObject.GetComponent<Movement>().playerSpeed = 0;
                Invoke("loadLose", 1f);
            }
        }
        healthBar.SetHP(health);
    }

    private void loadLose()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void DamageTake(int damage)
    {
        health -= damage;

        GameObject gameObject = Instantiate(PopupDamager,transform.position, Quaternion.identity);
        gameObject.GetComponent<damagePopup>().Setup(damage);

        if (gameObject.CompareTag("Monster"))
        {
            // Start the coroutine that waits for the animation to finish
            StartCoroutine(WaitForAnimation("Hit"));
        }
       }

    IEnumerator WaitForAnimation(string animationName)
    {
        Monster test = GetComponent<Monster>();
        
        // Wait until the current animation is finished playing
        yield return new WaitForSeconds(test.SetAnimation(animationName));
    }

    IEnumerator WaitForDeathAnimation(string animationName)
    {
        if (MonDeathSound != null && !isPlayed)
        {
            AudioInstance.Play(MonDeathSound);
            isPlayed = true;
        }
        Debug.Log("DeathSound");
        Monster test = GetComponent<Monster>();

        // Wait until the current animation is finished playing
        yield return new WaitForSeconds(test.SetAnimation(animationName));

        gameObject.GetComponent<Monster>().speed = 0;

        // Destroy the GameObject
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.CompareTag("Attack"))
        {
            //DamagerTake(damage)
        }*/
    }
}
