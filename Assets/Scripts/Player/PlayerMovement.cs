using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerMovement : MonoBehaviour
{
    public Action<float, float> OnShockwave;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Animator shockAnimator;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashColdown = 5.0f;
    [SerializeField] private float initialJumpForce = 5f;
    [SerializeField] private float holdJumpForce = 2f;
    [SerializeField] private float maxJumpTime = 0.75f;
    [SerializeField] private float dropForce = 1.0f;
    [SerializeField] private float shockwaveSpeed = 15.0f;
    [SerializeField] private float shockwaveStartDist = 3.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private float flippedTranslate = 2.5f;
    [SerializeField] private float wallJumpDelay = 0.5f;
    [SerializeField] private float soundDistance = 0.5f;



    private float dirX = 0;
    private float lastDirX = 0;
    private float jumpTime;
    private float wallJumpTime = 0;

    private bool isDashing;
    private float timeDash = 0;
    private bool soundPlayed = false;

    private bool isDropping = false;
    private bool isFlipped = false;
    private bool jumpPressed;
    private bool additionalJumpForceRequired;
    private bool dropPressed;

    private int health;
    public int fullHealth;
    private int noHearts;
    static public bool isDamaged;

    public int damagePerHit;
    public int damage;

    public float secondsCoolDown;
    public float secondsAttackAnimation;

    private enum MovementState { idle, running, jumping, falling, damage, death, dashing, down }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shockAnimator = transform.Find("Shockwave").GetComponent<Animator>();
        rb.freezeRotation = true;

        noHearts = 3;
        health = fullHealth;
        isDamaged = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused && noHearts > 0)
        {
            // Get inputs
            if (Input.GetKey(KeyCode.A))
            {
                dirX = -1;
                lastDirX = dirX;

                if (isFlipped == false)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;

                    isFlipped = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dirX = 1;
                lastDirX = dirX;

                if( isFlipped == true )
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;

                    isFlipped = false;
                }
            }
            else
                dirX = 0;

            // Dash on Shift press
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time > timeDash)
            {
                StartCoroutine(Dash());
            }

            // Jump on W press
            if (Input.GetKeyDown(KeyCode.W) && (IsGrounded() && (IsObstacleInFront() && !IsGroundedDown())))
            {
                jumpPressed = true;
            }

            // Apply additional force if W is held
            if (Input.GetKey(KeyCode.W) && !IsObstacleOnTop())
            {
                additionalJumpForceRequired = true;
            }

            // Make on S drop
            if (Input.GetKeyDown(KeyCode.S) && !IsOverGround(shockwaveStartDist))
            {
                dropPressed = true;
            }
            else if (!Input.GetKey(KeyCode.S) && isDropping)
            {
                isDropping = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StopAndAttack(secondsAttackAnimation));
            }

            UpdateAnimationState();
        }
    }

    private void FixedUpdate()
    {
        if (noHearts > 0)
        {
            // Modified Input
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

            if (jumpPressed)
            {
                if (IsObstacleInFront() && !IsGroundedDown() && Time.time > wallJumpTime)
                {
                    wallJumpTime = Time.time + wallJumpDelay;
                    rb.velocity = new Vector3(rb.velocity.x, initialJumpForce);
                    jumpTime = Time.time + maxJumpTime;
                }

                if (IsGrounded())
                {
                    rb.velocity = new Vector3(rb.velocity.x, initialJumpForce);
                    jumpTime = Time.time + maxJumpTime;
                    jumpPressed = false; // Reset after use
                }
            }

            if (additionalJumpForceRequired)
            {
                if (Time.time < jumpTime)
                {
                    rb.AddForce(new Vector2(0, holdJumpForce), ForceMode2D.Force);
                }

                if (IsObstacleInFront() && !IsGroundedDown() && Time.time > wallJumpTime)
                {
                    rb.velocity = new Vector3(rb.velocity.x, initialJumpForce);
                    jumpTime = Time.time + maxJumpTime;
                    wallJumpTime = Time.time + wallJumpDelay;
                }

                if (IsGrounded())
                {
                    rb.velocity = new Vector3(rb.velocity.x, initialJumpForce);
                    jumpTime = Time.time + maxJumpTime;
                }

                additionalJumpForceRequired = false;
                // Reset after use
            }

            if (dropPressed)
            {
                isDropping = true;
                soundPlayed = false;
                rb.AddForce(new Vector2(0, -dropForce), ForceMode2D.Impulse);
                dropPressed = false; // Reset after use
            }

            if (isDropping && soundStarter() && Math.Abs(rb.velocity.y) >= shockwaveSpeed) // verify velocity later
            {

            }

            if ((isDropping && IsOverGround(shockwaveStartDist)) && Math.Abs(rb.velocity.y) >= shockwaveSpeed)
            {
                audioSource.Play();
                soundPlayed = true;
                OnShockwave?.Invoke(5, 0.3f);
                anim.SetTrigger("Down");
                shockAnimator.SetTrigger("Shock");
                Debug.Log("Shockwave!");
                isDropping = false;
            }
            else
            {
                anim.ResetTrigger("Down");
                shockAnimator.ResetTrigger("Shock");
            }
        }
    }


    private IEnumerator Dash()
    {
        isDashing = true;
        audioSource2.Play();
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
            //sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            //sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .15f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.15f)
        {
            state = MovementState.falling;
        }

        if (isDashing)
        {
            state = MovementState.dashing;
        }
        if(noHearts < 1)
        {
            state = MovementState.death;
        }
        anim.SetInteger("State", (int)state);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .4f, jumpableGround).collider != null;
    }

    private bool soundStarter()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        if (hit.collider != null) 
        {
            float distance = hit.distance;
            if(distance <= soundDistance)
                return true; 
        }
        return false; 
    }

    private bool IsGroundedDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + 0.2f, jumpableGround);
        return hit.collider != null;
    }

    private bool IsOverGround(float dist)
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, dist, jumpableGround).collider != null;
    }

    private bool IsObstacleInFront()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size - new Vector3(0, 0.1f), 0f, new Vector2(dirX, 0), 0.1f, jumpableGround).collider != null;
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

    public void TakeDamage(int damage)
    {
        Debug.Log("no hearts: " + noHearts);
        anim.SetTrigger("IsHit");
        health -= damage;
        if (noHearts >= 0 && health <= 0)
        {
            noHearts--;
            health = fullHealth;
        }
        if (noHearts > 0) anim.SetInteger("State", 0);
        if (noHearts == 0 || health < 0) { 
            health = -1;
            anim.SetInteger("State", 5);
        }
    }
    IEnumerator StopAndAttack(float seconds)
    {
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(seconds);

        anim.SetInteger("State", 1);

    }

}


