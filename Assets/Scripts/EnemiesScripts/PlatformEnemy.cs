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
    public bool attack;
    public int direction;


    static public bool turn = false;

    private bool flipped;
    private bool move = true;
    private bool isInTurn = false;
    private int noTurns = 0;
    void Start()
    {
        animator = GetComponent<Animator>();
        turn = false;
        move = true ;

        noTurns = 0;
    }
    void Update()
    {
        Debug.Log(turn);
        if (turn == true && !isInTurn)
        {
            isInTurn = true;
            Debug.Log("is turn time");
            StartCoroutine(Flip(1));
            noTurns++;
            Debug.Log(noTurns);
            //animator.SetInteger("State", 0);
        }

        if (attack == false && turn == false)
        {
            animator.SetInteger("State", 1);
            move = true;
        }
        if (attack == true)
        {
            StartCoroutine(StopAndAttack(1.2f));
        }

        if (direction == 1 && enemy.velocity == Vector2.zero && move && !attack && !turn)
        {
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.right * speed;
        }
        else if (direction == -1 && enemy.velocity == Vector2.zero && move && !attack && !turn)
        {
            animator.SetInteger("State", 1);
            enemy.velocity = Vector2.left * speed;
        }

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
        Debug.Log("is in flip");
        enemy.velocity = Vector2.zero;
        move = false;

        animator.SetInteger("State", 0);

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
        isInTurn = false;
    }


}
