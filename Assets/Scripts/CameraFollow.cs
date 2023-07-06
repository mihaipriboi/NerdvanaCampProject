using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float cameraBottom;
    [SerializeField] private float cameraTop;
    [SerializeField] private float cameraLeft;
    [SerializeField] private float cameraRight;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > cameraTop)
        {
            GetComponent<CinemachineCameraOffset>().m_Offset.y = -transform.position.y + cameraTop;
        } 
        else if (transform.position.y < cameraBottom)
        {
            GetComponent<CinemachineCameraOffset>().m_Offset.y = -transform.position.y + cameraBottom;
        }
    }
}
