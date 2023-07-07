using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class disableToFar : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform sage;
    private float actionDistance = 10f;
    private Vector3 initPos;

    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (sage != null)
        {
            float distance = Vector3.Distance(transform.position, initPos);

            if (distance > actionDistance)
            {
                Debug.Log("aipath false");
                // Disable AiPath script
                //GetComponent<AIPath>().enabled = false;
                GetComponent<AIPath>().canMove = false;
            }
            else
            {
                // Enable AiPath script
                //GetComponent<AIPath>().enabled = true;

                GetComponent<AIPath>().canMove = true;
            }
        }
    }

}
