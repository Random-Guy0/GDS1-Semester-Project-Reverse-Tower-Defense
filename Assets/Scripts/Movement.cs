using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed =10.0f;
    private float gravityValue = -9.81f;
    public GameObject Camera;
    public string cameraS;
    public Vector3 move;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        cameraS = Camera.GetComponent<camera>().getCurrentCamera();
    }

    void Update()
    {
        cameraS = Camera.GetComponent<camera>().getCurrentCamera();
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        switch (cameraS)
        {
            case "AngleOne":
                move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
                controller.Move(move * Time.deltaTime * playerSpeed);
                break;
            case "AngleTwo":
                move = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")).normalized;
                controller.Move(move * Time.deltaTime * playerSpeed);
                break;
            case "AngleThree":
                move = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")).normalized;
                controller.Move(move * Time.deltaTime * playerSpeed);
                break;
            case "AngleFour":
                move = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")).normalized;
                controller.Move(move * Time.deltaTime * playerSpeed);
                break;
        }

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }


}
