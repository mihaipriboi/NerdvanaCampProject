using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    public int damagePerHit;
    public int damage;

    public float secondsCoolDown;
    public float secondsAttackAnimation;
    void Start()
    {
        animator = GetComponent<Animator>();
        damage = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StopAndAttack(secondsAttackAnimation));
        }
    }

    IEnumerator StopAndAttack( float seconds )
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(seconds);

        animator.SetInteger("State", 1);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("player attaked");
        if (collision.gameObject.tag == "enemyCollider")
        {
            Debug.Log("the enemy is attacked");
            StartCoroutine(GiveDamage(collision.gameObject, secondsCoolDown));
        }
    }

    IEnumerator GiveDamage(GameObject enemy, float seconds)
    {
        Debug.Log("Collision");
        damage += damagePerHit;
        Debug.Log(damage);
        enemy.GetComponent<PlatformEnemy>().TakeDamage(damagePerHit);
        damage = 0;
        yield return new WaitForSeconds(seconds);
    }
}
