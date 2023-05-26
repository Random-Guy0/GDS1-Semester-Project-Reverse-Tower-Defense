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
        else if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 delta = mouseDelta;
            delta *= -1;
            transform.Translate(Time.deltaTime * moveSensitivity * delta);
        }
        else if (Input.mouseScrollDelta != Vector2.zero)
        {
            Vector3 delta = Vector3.forward * Input.mouseScrollDelta.y;
            transform.Translate(Time.deltaTime * zoomSensitivity * delta);
        }

        previousMousePosition = currentMousePosition;
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