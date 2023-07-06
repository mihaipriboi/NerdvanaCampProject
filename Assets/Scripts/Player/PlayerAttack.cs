using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int damagePerHit;
    public int damage;

    public float secondsCoolDown;
    void Start()
    {
        damage = 0;
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{gameObject} collided with {collision.gameObject}");
        if (collision.gameObject.tag == "enemyCollider")
        {
            StartCoroutine(GiveDamage(collision.gameObject, secondsCoolDown));
        }
    }

    IEnumerator GiveDamage(GameObject enemy, float seconds)
    {
        //Debug.Log("Collision");
        damage += damagePerHit;
        //Debug.Log(damage);
        enemy.GetComponent<PlatformEnemy>().TakeDamage(damagePerHit);
        damage = 0;
        yield return new WaitForSeconds(seconds);
    }
}
