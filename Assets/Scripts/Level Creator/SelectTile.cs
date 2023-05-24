using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;

    private UserLevelCreator levelCreator;

    private void Start()
    {
        levelCreator = FindObjectOfType<UserLevelCreator>();
    }

    private void OnMouseEnter()
    {
        levelCreator.SetSelectedTile(transform.position);
        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            material.shader = Shader.Find("Custom/Outline");
            material.SetColor("_OutlineColor", new Color(1.0f, 1.0f, 0.0f));
            material.SetFloat("_OutlineWidth", 1.1f);
        }

        renderer.materials = materials;
    }

    private void OnMouseExit()
    {
        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            material.shader = Shader.Find("Standard");
        }

        renderer.materials = materials;
    }
}
