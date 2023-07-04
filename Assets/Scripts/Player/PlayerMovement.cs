using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Action<float, float> OnShockwave;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashColdown = 5.0f;
    [SerializeField] private float initialJumpForce = 5f;
    [SerializeField] private float holdJumpForce = 2f;
    [SerializeField] private float maxJumpTime = 0.75f;
    [SerializeField] private float dropForce = 1.0f;
    [SerializeField] private AudioSource audioSource;


    private float dirX = 0;
    private float lastDirX = 0;
    private float jumpTime;

    private bool isDashing;
    private float timeDash = 0;

    private bool isDropping = false;

    private enum MovementState { idle, running, jumping, falling, dashing }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // Modified Input
        if (Input.GetKey(KeyCode.A))
        {
            dirX = -1;
            lastDirX = dirX;
        }  
        else if (Input.GetKey(KeyCode.D))
        {
            dirX = 1;
            lastDirX = dirX;
        }
        else
            dirX = 0;

        if (!isDashing)
        {
            if (IsObstacleInFront())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            }
        }

        // Dash on Shift press
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time > timeDash)
        {
            StartCoroutine(Dash());
        }

        // Jump on W press
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, initialJumpForce);
            jumpTime = Time.time + maxJumpTime;
        }


        // Apply additional force if W is held
        if (Input.GetKey(KeyCode.W) && !IsObstacleOnTop())
        {
            if (Time.time < jumpTime)
            {
                rb.AddForce(new Vector2(0, holdJumpForce), ForceMode2D.Force);
            }

            if (IsGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, initialJumpForce);
                jumpTime = Time.time + maxJumpTime;
            }
        }

        // Make on S drop
        if(Input.GetKeyDown(KeyCode.S) && !IsGrounded())
        {
            isDropping = true;
            audioSource.Play();
            rb.AddForce(new Vector2(0, -dropForce), ForceMode2D.Impulse);
        }

        UpdateAnimationState();

        if (isDropping && IsGrounded())
        {
            OnShockwave?.Invoke(10, 0.5f);
            isDropping = false;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.AddForce(new Vector2(lastDirX * dashForce, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        timeDash = Time.time + dashColdown;
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        if (isDashing)
        {
            state = MovementState.dashing;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround).collider != null;
    }

    private bool IsObstacleInFront()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, new Vector2(dirX, 0), 0.05f, jumpableGround).collider != null;
    }
    private bool IsObstacleOnTop()
    {
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, Vector2.up, 1f, jumpableGround);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

}


