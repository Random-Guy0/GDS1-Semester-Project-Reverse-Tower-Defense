using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLevelCreatorCamera : MonoBehaviour
{
    [SerializeField] private UserLevelCreator levelCreator;
    [SerializeField] private float moveSensitivity;
    [SerializeField] private float orbitSensitivity;
    [SerializeField] private float orbitSnapSpeed;
    [SerializeField] private float zoomSensitivity;
    
    private Vector3 previousMousePosition;

    private float upDownKeys = 0f;
    private Vector2 wasd = Vector2.zero;

    private bool rotating = false;

    private void Start()
    {
        Vector3 position = transform.position;
        position.x = levelCreator.GetWidth() * 0.5f;
        transform.position = position;
        
        previousMousePosition = Input.mousePosition;
        
        Vector3 lookAt = levelCreator.GetCenter();
        transform.LookAt(lookAt);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            upDownKeys += 1f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            upDownKeys -= 1f;
        }

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
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 delta = mouseDelta;
            delta *= -1;

            Vector3 lookAt = levelCreator.GetCenter();
            Vector3 lookDir = (lookAt - transform.position).normalized;
            float dot = Vector3.Dot(lookDir, transform.forward);

            if (dot > 0.99f && !rotating)
            {
                transform.LookAt(lookAt);
                transform.Translate(Time.deltaTime * orbitSensitivity * delta);
            }
            else
            {
                if (!rotating)
                {
                    StartCoroutine(LerpRotation(lookAt));
                }
                transform.Translate(Time.deltaTime * orbitSensitivity * delta);
            }
        }
        else if (Input.GetKey(KeyCode.Mouse2) || !wasd.Equals(Vector2.zero))
        {
            Vector3 delta = Vector3.zero;
            if (Input.GetKey(KeyCode.Mouse2))
            {
                delta = mouseDelta;
                delta *= -1;
            }

            delta += (Vector3)wasd;
            transform.Translate(Time.deltaTime * moveSensitivity * delta);
        }
        else if (Input.mouseScrollDelta != Vector2.zero || upDownKeys != 0f)
        {
            Vector3 delta = Vector3.forward * (Input.mouseScrollDelta.y + upDownKeys);
            transform.Translate(Time.deltaTime * zoomSensitivity * delta);
        }

        previousMousePosition = currentMousePosition;

        upDownKeys = 0f;
        wasd = Vector2.zero;
    }

    private IEnumerator LerpRotation(Vector3 lookAt)
    {
        rotating = true;
        float time = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion rotation = Quaternion.LookRotation(lookAt - transform.position);
        while (time < orbitSnapSpeed)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, rotation, time / orbitSnapSpeed);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = rotation;

        rotating = false;
    }
}