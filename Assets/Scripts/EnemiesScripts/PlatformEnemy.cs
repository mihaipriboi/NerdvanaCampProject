using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{

    public float speed;
    public float flippedTranslate;
    public Animator animator;
    public Rigidbody2D enemy;
    static public bool attack = false;


    private int direction;
    private bool move = false;

    private bool flipped;
    private bool turn = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && !turn)
        {
           // Debug.Log("roiwri");
            attack = true;
        }

        if (attack == false && turn == false )
        {
            //.ResetTrigger("Attack");
            animator.SetInteger("State", 1);
            move = true;
        }
        if ( attack == true )
        {
            //Debug.Log("mda");
            StartCoroutine(StopAndAttack(2.5f));
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

        //Debug.Log(direction);

        if( direction == 1 && enemy.velocity == Vector2.zero && move && !attack )
        {
            //Debug.Log("stop");
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.right * speed;
        }
        else if( direction == -1 && enemy.velocity == Vector2.zero && move && !attack)
        {
            //Debug.Log("stop");
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.left * speed;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggered");
        move = false;
        turn = true;
        enemy.velocity = Vector2.zero;
        animator.SetInteger("State", 0);

        StartCoroutine(Flip(2));
    }

    IEnumerator StopAndAttack( float seconds ) {
        Debug.Log("Attack");
        move = false;
        Debug.Log("Attack");
        enemy.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(seconds);

        Debug.Log("after attack");

        animator.ResetTrigger("Attack");
        animator.SetInteger("State", 1);

        yield return new WaitForSeconds(0.2f);

        attack = false;
        move = true;

        Debug.Log("Done attack!");
    }

    IEnumerator Flip( int seconds)
    {
        //Debug.Log("flip before wait");
        yield return new WaitForSeconds(seconds);

        //Debug.Log("flip after wait");

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (direction == 1)
        {
            enemy.transform.position = enemy.transform.position + Vector3.left * flippedTranslate;
        }
        else
        {
            enemy.transform.position = enemy.transform.position + Vector3.right * flippedTranslate;
        }

        direction *= -1;

        move = true;
        turn = false;

    }


}
