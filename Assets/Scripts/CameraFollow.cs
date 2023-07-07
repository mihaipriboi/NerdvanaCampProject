using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float cameraBottom;
    [SerializeField] private float cameraTop;
    [SerializeField] private float cameraLeft;
    [SerializeField] private float cameraRight;

    private CinemachineVirtualCamera virtualCamera;
    private float offsetY = 0f;


    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (virtualCamera.m_Lens.OrthographicSize > 7.5)
        {
            offsetY = -2f;
        } else
        {
            offsetY = 2f;
        }

        if (transform.position.y > cameraTop)
        {
            GetComponent<CinemachineCameraOffset>().m_Offset.y = -transform.position.y + cameraTop;
        } 
        else if (transform.position.y < cameraBottom)
        {
            GetComponent<CinemachineCameraOffset>().m_Offset.y = -transform.position.y + (cameraBottom - offsetY);
        }
    }
}
