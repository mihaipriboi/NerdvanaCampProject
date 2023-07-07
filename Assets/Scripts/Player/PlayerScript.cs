using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int health;
    private int noHearts;
    static public bool isDamaged;


    public Animator animator;

    public int damagePerHit;
    public int damage;

    public float secondsCoolDown;
    public float secondsAttackAnimation;

    void Start()
    {
        noHearts = 3;
        isDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StopAndAttack(secondsAttackAnimation));
        }
    }

    public void TakeDamage( int damage )
    {
        //Debug.Log("pula");
        animator.SetInteger("State", 4);
        //Debug.Log(damage);
        if (health - damage > 0)
        {
            health -= damage;
        }
        else if(noHearts >= 0)
        {
            noHearts--;
        }
        animator.SetInteger("State", 0);
    }

    IEnumerator StopAndAttack(float seconds)
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(seconds);

        animator.SetInteger("State", 1);

    }

}