using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public GameObject slow;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private PlayerHealth PlayerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (manaManager.ableToCost(20))
            {
                manaManager.costMana(20);
                Instantiate(slow, gameObject.transform.position, gameObject.transform.rotation);
            }
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            if (manaManager.ableToCost(50))
            {
                manaManager.costMana(50);
                PlayerHealth.health = PlayerHealth.maxHealth;
            }
        }
    }
}
