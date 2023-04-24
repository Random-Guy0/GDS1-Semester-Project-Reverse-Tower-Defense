using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hovering : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public GameObject HoverPanel;


    public void OnPointerEnter(PointerEventData eventData)
    {

            HoverPanel.SetActive(true);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HoverPanel.SetActive(false);
    }
}
