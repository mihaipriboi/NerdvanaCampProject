using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TutorialEnd : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider2D boxCollider2D;
    private bool isActive = false;
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float transitionDuration = 1f;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isActive)
        {
            isActive = true;
            StartCoroutine(ChangeCameraSize(virtualCamera, 9, transitionDuration));
            Debug.Log("caca");
        }
    }

    IEnumerator ChangeCameraSize(CinemachineVirtualCamera vcam, float targetSize, float duration)
    {
        float startSize = vcam.m_Lens.OrthographicSize;
        float elapsed = 0;

        while (elapsed < duration)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the target size is set when the transition is done
        vcam.m_Lens.OrthographicSize = targetSize;
    }
}
