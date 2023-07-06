using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int health;
    private int noHearts;
    static public bool isDamaged;


    public Animator animator;
    void Start()
    {
        noHearts = 3;
        isDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage( int damage )
    {
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

}