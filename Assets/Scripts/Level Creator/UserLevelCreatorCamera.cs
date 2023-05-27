using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLevelCreatorCamera : MonoBehaviour
{
    [SerializeField] private UserLevelCreator levelCreator;
    [SerializeField] private float moveSensitivity;
    [SerializeField] private float orbitSensitivity;
    [SerializeField] private float zoomSensitivity;
    
    private Vector3 previousMousePosition;
    
    private Vector2 wasd = Vector2.zero;

    private bool rotating = false;

    private void Start()
    {
        ResetPosition();
        
        previousMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            wasd.y += 1f;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            wasd.y -= 1f;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            wasd.x += 1f;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            wasd.x -= 1f;
        }
    }

    private void LateUpdate()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - previousMousePosition;
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 delta = new Vector3(mouseDelta.y * -1f, mouseDelta.x, 0f);

            Vector3 rotation = orbitSensitivity * Time.deltaTime * delta;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            rotation += currentRotation;
            transform.rotation = Quaternion.Euler(rotation);
        }
        
        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 delta = mouseDelta * -1f;
            
            transform.Translate(Time.deltaTime * moveSensitivity * delta);
        }

        if (!wasd.Equals(Vector2.zero))
        {
            Vector3 delta = new Vector3(wasd.x, 0f, wasd.y);

            transform.Translate(Time.deltaTime * moveSensitivity * delta);
        }
        
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Vector3 delta = Vector3.forward * Input.mouseScrollDelta.y;
            transform.Translate(Time.deltaTime * zoomSensitivity * delta);
        }

        previousMousePosition = currentMousePosition;
        
        wasd = Vector2.zero;
    }

    private void ResetPosition()
    {
        Vector3 position = transform.position;
        position.x = levelCreator.GetWidth() * 0.5f;
        position.y = 15f;
        position.z = -15f;
        transform.position = position;
        
        Vector3 lookAt = levelCreator.GetCenter();
        transform.LookAt(lookAt);
    }
}