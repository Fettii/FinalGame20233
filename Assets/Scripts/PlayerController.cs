using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    public float punchRange = 1f;
    public float kickRange = 1.5f;
    public int punchDamageAmount = 1;
    public int kickDamageAmount = 2;
    public bool isPlayer2 = false;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isBlocking = false;
    private bool isGrounded = false;
    private bool isDashing = false;
    private float lastDashTime = 0f;
    private bool isPunching = false;
    private bool isKicking = false;

    public static event Action<bool> OnBlockingStateChanged;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {

        if (isPlayer2)
        {

            if (Input.GetKeyDown(KeyCode.Keypad8) && isGrounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
            }


            if (Input.GetKey(KeyCode.Keypad4))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                animator.SetFloat("Speed", Mathf.Abs(-1f)); // Flip the sprite
            }


            if (Input.GetKey(KeyCode.Keypad6))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                animator.SetFloat("Speed", Mathf.Abs(1f)); // Unflip the sprite
            }


            if (Input.GetKeyDown(KeyCode.Keypad7) && !isPunching)
            {
                animator.SetTrigger("Punch");
                isPunching = true;
            }


            if (Input.GetKeyDown(KeyCode.Keypad9) && !isKicking)
            {
                animator.SetTrigger("Kick");
                isKicking = true;
            }
            //player 1
        }
        else
        {

            // Movement
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

            // Jump
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
            }



            // Block
            if (Input.GetKey(KeyCode.A))
            {
                isBlocking = true;
                OnBlockingStateChanged?.Invoke(true);
            }
            else
            {
                isBlocking = false;
                OnBlockingStateChanged?.Invoke(false);
            }

            // Punch
            if (Input.GetKeyDown(KeyCode.O) && !isPunching)
            {
                animator.SetTrigger("Punch");
                isPunching = true;
                DealDamageToPlayer2(punchDamageAmount);
            }

            // Kick
            if (Input.GetKeyDown(KeyCode.P) && !isKicking)
            {
                animator.SetTrigger("Kick");
                isKicking = true;
                DealDamageToPlayer2(kickDamageAmount);
            }
        }

    }

    private void DealDamageToPlayer2(int damageAmount)
    {
        GameObject player2 = GameObject.Find("Player2");
        Health player2Health = player2.GetComponent<Health>();

        player2Health.ApplyDamage(damageAmount);
    }


    private void EndPunch()
    {
        isPunching = false;
        animator.ResetTrigger("Punch");
    }


    private void EndKick()
    {
        isKicking = false;
        animator.ResetTrigger("Kick");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }


}
