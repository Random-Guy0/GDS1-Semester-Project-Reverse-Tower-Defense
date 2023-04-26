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
    public Animator animator;
    public float health;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        cameraS = Camera.GetComponent<camera>().getCurrentCamera();
    }

    void Update()
    {
        health = GetComponent<PlayerHealth>().health;
        cameraS = Camera.GetComponent<camera>().getCurrentCamera();
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if(health <= 0)
        {
            animator.SetTrigger("Death");
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
            animator.SetBool("IsMove", true);
            gameObject.transform.forward = move;
        }
        else if(move == Vector3.zero)
        {
            animator.SetBool("IsMove", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mana"))
        {
            animator.SetBool("IsPickUp", true);
        }
    }


}
