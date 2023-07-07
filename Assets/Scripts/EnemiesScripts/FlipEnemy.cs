using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipEnemy : MonoBehaviour
{
    public Rigidbody2D enemy;
    private bool isFlliped;
    void Start()
    {
        isFlliped = false;
        enemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = enemy.velocity;
        //Debug.Log(enemy.velocity.x);
        if (enemy.velocity.x < 0)
        {
        }
        if( isFlliped == true  && enemy.velocity.x > 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            isFlliped = false;
        }
    }
}
