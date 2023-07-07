using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class PlatformEnemy : MonoBehaviour
{

    public float speed;
    public float flippedTranslate;
    public Animator animator;
    public Rigidbody2D enemy;
    static public bool attack;
    public int direction;
    public int attackType;
    public float attackTime;
    public int damagePerHit;

    public int enemyHealth;


    static public bool turn = false;

    private bool flipped;
    private bool move = true;
    private bool isInTurn = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        turn = false;
        move = true ;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), FindObjectOfType<PlayerScript>().GetComponent<Collider2D>(), false);
    }
    void Update()
    {
        if (enemyHealth > 0)
        {
            if (attack == true)
            {
                if (attackType != -1)
                {
                    animator.SetInteger("AttackType", attackType);
                }
                StartCoroutine(StopAndAttack(attackTime));
            }
        }
    }

    IEnumerator StopAndAttack( float seconds) {
        move = false;
        enemy.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(seconds);
        animator.SetInteger("State", 1);

        attack = false;
        move = true;
    }

    IEnumerator Flip( int seconds)
    {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject} collided with {collision.gameObject}", collision.gameObject);

        if (collision.gameObject.tag == "Player" && collision.gameObject.tag != "PlayerWeapon")
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damagePerHit);
        }
    }

    public void TakeDamage( int damage )
    {
        animator.SetTrigger("IsHit");
        //Debug.Log(damage);
        if (enemyHealth - damage > 0)
        {
            enemyHealth -= damage;
        }
        else if (enemyHealth - damage <= 0)
        {
            enemyHealth = -1;
            animator.SetInteger("State", -1);
        }
    }
}
