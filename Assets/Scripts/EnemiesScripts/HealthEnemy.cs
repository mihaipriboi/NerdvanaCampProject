using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class HealthEnemy : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D enemy;
    public int damagePerHit;

    public int enemyHealth;
    void Start()
    {
        animator = GetComponent<Animator>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), FindObjectOfType<PlayerScript>().GetComponent<Collider2D>(), false);
    }
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("mda");
        Debug.Log($"{gameObject} collided with {collision.gameObject}", collision.gameObject);

        if (collision.gameObject.tag == "Player" && collision.gameObject.tag != "PlayerWeapon")
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damagePerHit);
        }
    }

    public void TakeDamage(int damage)
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
