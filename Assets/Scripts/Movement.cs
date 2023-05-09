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
    public int gateHp;

    private void Start()
    {
        Camera = GameObject.Find("Main Camera");
        controller = gameObject.AddComponent<CharacterController>();
        cameraS = Camera.GetComponent<camera>().getCurrentCamera();
        gateHp = GameObject.Find("Game Manager").GetComponent<GameManager>().getLevelHealth();
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
            animator.SetBool("Death", true);
        }
    }

    private void Update()
    {
        gateHp = GameObject.Find("Game Manager").GetComponent<GameManager>().getLevelHealth();
        health = GetComponent<PlayerHealth>().health;
        groundedPlayer = controller.isGrounded;
        if (health == 0)
        {
            animator.SetBool("Death", true);
        }

        if (gateHp == 0)
        {
            Debug.Log("dance");
            animator.SetBool("Dance", true);
            health = 200;
        }

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
