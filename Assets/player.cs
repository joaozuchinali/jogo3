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

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
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

        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false; // Reset jump to prevent continuous jumping
        }
    }

    public void OnMoveLeftDown()
    {
        moveLeft = true;
        sr.flipX = true;
    }

    public void OnMoveLeftUp()
    {
        moveLeft = false;
    }

    public void OnMoveRightDown()
    {
        moveRight = true;
        sr.flipX = false;
    }

    public void OnMoveRightUp()
    {
        moveRight = false;
    }

    public void OnJump()
    {
        isJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            animator.SetBool("pulando", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("pulando", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "fruta")
        {
            if(collision.gameObject.GetComponent<Animator>().GetBool("coletando") == true) {
                return;
            }

            collision.gameObject.GetComponent<Animator>().SetBool("coletando", true);
            Destroy(collision.gameObject, 1f);
            GameController.setPontos(1);
        }

        if (collision.gameObject.tag == "inimigo")
        {
            morre();
        }

        if(collision.gameObject.tag == "end") {
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
