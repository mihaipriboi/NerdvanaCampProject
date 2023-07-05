using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{

    public float speed;
    public Animator animator;
    public Rigidbody2D enemy;
    static public bool attack = false;


    private int direction;
    private bool move = false;

    private bool flippepd;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            attack = true;
        }

        if (attack == false)
        {
            //.ResetTrigger("Attack");
            animator.SetInteger("State", 1);
            move = true;
        }
        if ( attack == true )
        {
            StopAndAttack();
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            direction = 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("State", 0);
            enemy.velocity = Vector2.zero;

        }

        Debug.Log(direction);

        if( direction == 1 && enemy.velocity == Vector2.zero && move )
        {
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.right * speed;
        }
        else if( direction == -1 && enemy.velocity == Vector2.zero && move )
        {
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.left * speed;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        enemy.velocity = Vector2.zero;
        animator.SetInteger("State", 0);

        StartCoroutine(waiter(20));

        Flip();
    }

    void StopAndAttack()
    {
        move = false;
        enemy.velocity = Vector2.zero;
        animator.SetInteger("State", 0);
        animator.SetTrigger("Attack");

        StartCoroutine(waiter(60));

        attack = false;
        //animator.ResetTrigger("Attack");
    }

    IEnumerator waiter( int seconds ) {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(seconds);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (direction == 1)
        {
            enemy.transform.position = enemy.transform.position + Vector3.left * 4.5f;
        }
        else
        {
            enemy.transform.position = enemy.transform.position + Vector3.right * 4.5f;
        }

        direction *= -1;

    }

    
}
