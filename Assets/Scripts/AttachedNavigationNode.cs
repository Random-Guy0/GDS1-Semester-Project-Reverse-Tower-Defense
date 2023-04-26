using System;
using UnityEngine;

public class AttachedNavigationNode : MonoBehaviour
{
    public NavigationNode navigationNode;

    private void Awake()
    {
        navigationNode = new NavigationNode(transform.position);
    }
}