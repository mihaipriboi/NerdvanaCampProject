using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUpdateCollider : MonoBehaviour
{
    void Update()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }
}
