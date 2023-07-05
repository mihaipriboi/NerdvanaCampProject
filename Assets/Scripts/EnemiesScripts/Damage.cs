using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int damagePerHit;
    static public int damage;
    void Start()
    {
        damage = 0;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        damage += damagePerHit;
        Debug.Log(damage);
    }
}
