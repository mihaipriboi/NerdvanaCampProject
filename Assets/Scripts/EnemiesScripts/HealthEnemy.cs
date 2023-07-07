using Pathfinding;
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

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), FindObjectOfType<HealthEnemy>().GetComponent<Collider2D>(), false);
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
        Debug.Log("Is hit");
        StartCoroutine(HitState(damage));
        
    }

    IEnumerator HitState(int damage)
    {
        animator.SetTrigger("IsHit");
        animator.ResetTrigger("IsNotHit");

        Debug.Log("mda"+ damage);
        enemyHealth -= damage;
        
        if (enemyHealth < 0)
        {
            enemyHealth = -1;
            Debug.Log("mdaksfdhbjs" + damage);
            animator.SetInteger("State", -1);
            GetComponent<AIPath>().enabled = false;
            GetComponent<AIPath>().Die();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        yield return new WaitForSeconds(0.1f);

        animator.SetTrigger("IsNotHit");
    }
}
