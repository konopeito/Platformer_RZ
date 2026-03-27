using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Gameplay Values")]
    [SerializeField] private int coinValue = 10;
    [SerializeField] private int enemyDamage = 10;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Input in Update
        horizontalInput = Input.GetAxis("Horizontal");
        CheckGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Keep current X velocity, apply jump on Y
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            //if (AudioManager.Instance != null)
            //{
            //    AudioManager.Instance.PlayJumpSound();
            //}
        }
    }

    private void FixedUpdate()
    {
        // Physics movement in FixedUpdate
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void CheckGrounded()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(coinValue);
            }

            if (CoinPoolManager.Instance != null)
            {
                CoinPoolManager.Instance.CollectCoin(other.gameObject);
            }
            else
            {
                // Fallback if pool manager isn't present
                other.gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(enemyDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}