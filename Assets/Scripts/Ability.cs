using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public GameObject slow;
    [SerializeField] private ManaManager manaManager;
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
    }
}
