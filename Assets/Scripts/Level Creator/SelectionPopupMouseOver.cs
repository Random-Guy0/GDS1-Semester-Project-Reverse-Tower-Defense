using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionPopupMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UserLevelCreator levelCreator;
    
    private bool mouseOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !mouseOver)
        {
            levelCreator.ClearSelection();
        }
    }
}
