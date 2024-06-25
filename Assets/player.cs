using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private GameObject endPannel;

    private bool moveLeft;
    private bool moveRight;
    private bool isJumping;
    private bool isGrounded;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        if (moveLeft)
        {
            sr.flipX = true;
        }
        else if (moveRight)
        {
            sr.flipX = false;
        }
    }

    private void HandleMovement()
    {
        if (moveLeft)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (moveRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (isJumping && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
            isGrounded = false;
        }
    }

    private void HandleAnimation()
    {
        animator.SetBool("correndo", moveLeft || moveRight);
        animator.SetBool("pulando", !isGrounded);
    }

    public void OnMoveLeftDown()
    {
        moveLeft = true;
    }

    public void OnMoveLeftUp()
    {
        moveLeft = false;
    }

    public void OnMoveRightDown()
    {
        moveRight = true;
    }

    public void OnMoveRightUp()
    {
        moveRight = false;
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "fruta")
        {
            if (collision.gameObject.GetComponent<Animator>().GetBool("coletando") == true)
            {
                return;
            }

            collision.gameObject.GetComponent<Animator>().SetBool("coletando", true);
            collision.gameObject.GetComponent<Collider2D>().enabled = false; // Desativa o Collider2D
            Destroy(collision.gameObject, 1f);
            GameController.setPontos(1);
        }

        if (collision.gameObject.tag == "inimigo")
        {
            morre();
        }

        if (collision.gameObject.tag == "end")
        {
            endPannel.SetActive(true);
            animator.SetBool("correndo", false);
            animator.SetBool("pulando", false);
        }
    }

    private void morre()
    {
        GameController.setPerda();
        transform.position = spawn.transform.position;
    }
}
