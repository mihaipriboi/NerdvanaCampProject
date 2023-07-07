using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y > 0)
            rb.velocity = Vector2.zero;
    }
}
