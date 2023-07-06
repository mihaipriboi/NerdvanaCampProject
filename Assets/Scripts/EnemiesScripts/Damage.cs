using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int damagePerHit;
    public int damage;

    public float seconds;
    void Start()
    {
        damage = 0;
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(GiveDamage(collision.gameObject, seconds));
        }
    }

    IEnumerator GiveDamage(GameObject player, float seconds)
    {
        Debug.Log("Collision");
        damage += damagePerHit;
        Debug.Log(damage);
        player.GetComponent<PlayerScript>().TakeDamage(damagePerHit);
        damage = 0;
        yield return new WaitForSeconds(seconds);
    }
}
