using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    private float playerInput;
    private Vector3 playerVec;
    private Rigidbody playerRb;
    private bool isGround = true;

    void Start()
    {
        if(!playerRb)
        {
            playerRb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround) 
        {
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }
    }
    
    void Move()
    {
        playerInput = Input.GetAxis("Horizontal");
        playerVec = new Vector3(playerInput, 0, 0).normalized;
        transform.position += playerVec * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
