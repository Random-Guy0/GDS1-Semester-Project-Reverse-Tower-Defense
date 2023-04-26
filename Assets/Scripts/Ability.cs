using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public GameObject slow;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private PlayerHealth PlayerHealth;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (manaManager.ableToCost(20))
            {
                manaManager.costMana(50);
                Instantiate(slow, gameObject.transform.position, gameObject.transform.rotation);
                animator.SetTrigger("Ability");
            }
        }else if(Input.GetKeyUp(KeyCode.Z)) {
            animator.ResetTrigger("Ability");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (manaManager.ableToCost(50))
            {
                manaManager.costMana(50);
                PlayerHealth.health = PlayerHealth.maxHealth;
                animator.SetTrigger("Ability");
            }
        }else if (Input.GetKeyUp(KeyCode.H))
        {
            animator.ResetTrigger("Ability");
        }
    }
}
