using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class damagePopup : MonoBehaviour
{
    public GameObject text;
    public int damagetxt;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += (Vector3.up)/100;
        text.GetComponent<TextMesh>().text = damagetxt.ToString();
    }

    public void Setup(int damageAmount)
    {
        damagetxt = damageAmount;
    }
}
