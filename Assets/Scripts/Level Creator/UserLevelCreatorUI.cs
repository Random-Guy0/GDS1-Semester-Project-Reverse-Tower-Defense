using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserLevelCreatorUI : MonoBehaviour
{
    [SerializeField] private UserLevelCreator levelCreator;
    [SerializeField] private GameObject levelCreatorMenu;
    [SerializeField] private GameObject levelSizeSubmenu;
    [SerializeField] private GameObject pathSubmenu;
    [SerializeField] private GameObject obstacleSubmenu;
    [SerializeField] private Image selectionToolButton;
    [SerializeField] private Image pathToolButton;
    [SerializeField] private Image archeryToolButton;

    private Image currentActiveButton;

    private void Start()
    {
        levelCreator.SetTool(0);
        currentActiveButton = selectionToolButton;
        currentActiveButton.color = new Color(89f / 255f, 89f / 255f, 89f / 255f);
    }

    public void OpenLevelSizeSubmenu()
    {
        levelCreatorMenu.SetActive(false);
        levelSizeSubmenu.SetActive(true);
    }

    public void OpenPathSubmenu()
    {
        levelCreatorMenu.SetActive(false);
        pathSubmenu.SetActive(true);
        levelCreator.SetTool(3);
        SetActiveButton(pathToolButton);
    }

    public void OpenObstacleSubmenu()
    {
        levelCreatorMenu.SetActive(false);
        obstacleSubmenu.SetActive(true);
        levelCreator.SetTool(6);
        SetActiveButton(archeryToolButton);
    }

    public void CloseSubmenu(GameObject submenu)
    {
        submenu.SetActive(false);
        levelCreatorMenu.SetActive(true);
        levelCreator.SetTool(0);
        SetActiveButton(selectionToolButton);
    }

    public void SetActiveButton(Image button)
    {
        currentActiveButton.color = new Color(1, 1, 1);
        button.color = new Color(89f / 255f, 89f / 255f, 89f / 255f);
        currentActiveButton = button;
    }
}
