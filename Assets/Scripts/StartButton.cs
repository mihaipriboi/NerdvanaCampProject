using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject manager;

    private void Update()
    {
        if (Input.anyKey)
        {
            manager.GetComponent<GameManager>().StartGame();
        }
    }
}
