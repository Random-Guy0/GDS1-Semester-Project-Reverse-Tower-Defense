using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 5;
    private float gravityValue = -9.81f;
    public GameObject Camera;
    public string cameraS;
    public Vector3 move;
    public Animator animator;
    public float health;

    private void Start()
    {
        Camera = GameObject.Find("Main Camera");
        controller = gameObject.AddComponent<CharacterController>();
        controller.stepOffset = 0.2f;
        cameraS = Camera.GetComponent<camera>().getCurrentCamera();
    }

    private void Move()
    {
        controller.Move((move + playerVelocity) * Time.deltaTime * playerSpeed);
        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    private void UpdateAnimator()
    {
        if (move != Vector3.zero)
        {
            animator.SetBool("IsMove", true);
            gameObject.transform.forward = move;
        }
        else
        {
            animator.SetBool("IsMove", false);
        }

        if (health <= 0)
        {
            animator.SetTrigger("Death");
        }
    }

    private void Update()
    {
        health = GetComponent<PlayerHealth>().health;
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            playerVelocity.y = 0f;
        }

        cameraS = Camera.GetComponent<camera>().getCurrentCamera();

        switch (cameraS)
        {
            case "AngleOne":
                move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                break;
            case "AngleTwo":
                move = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
                break;
            case "AngleThree":
                move = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
                break;
            case "AngleFour":
                move = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
                break;
        }

        Move();
        UpdateAnimator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mana"))
        {
            animator.SetBool("IsPickUp", true);
        }
    }


}
