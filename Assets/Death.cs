using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("death Tile");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().noHearts = -1;
            //collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(200);
        }
    }
}
