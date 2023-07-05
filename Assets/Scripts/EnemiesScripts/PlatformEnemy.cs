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
            attack = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("State", 0);
            enemy.velocity = Vector2.zero;

        }

        if (turn == true)
            animator.SetInteger("State", 0);

        if (attack == false && turn == false )
        {
            animator.SetInteger("State", 1);
            move = true;
        }
        if ( attack == true )
        {
            StartCoroutine(StopAndAttack(1.2f));
        }

        if( direction == 1 && enemy.velocity == Vector2.zero && move && !attack && !turn)
        {
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.right * speed;
        }
        else if( direction == -1 && enemy.velocity == Vector2.zero && move && !attack && !turn)
        {
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.left * speed;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        turn = true;
        move = false;

        animator.SetInteger("State", 0);
        enemy.velocity = Vector2.zero;

        StartCoroutine(Flip(1));
    }

    IEnumerator StopAndAttack( float seconds ) {
        move = false;
        enemy.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(seconds);

        animator.ResetTrigger("Attack");
        animator.SetInteger("State", 1);

        attack = false;
        move = true;
    }

    IEnumerator Flip( int seconds)
    {
        yield return new WaitForSeconds(seconds);

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
