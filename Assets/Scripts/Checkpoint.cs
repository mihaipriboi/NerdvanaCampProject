using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Checkpoint : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Animator anim;
    private SpriteRenderer sprite;

    private bool isActive = false;

    [SerializeField] private int CheckpointID;


    private enum MovementState { idle, checkpoint};

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2D.isTrigger = true;
    }

    void Update()
    {
        // Update is called once per frame
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (isActive)
        {
            state = MovementState.checkpoint;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
            sprite.flipX = true;
        }
        anim.SetInteger("State", (int)state);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isActive = true;
            Debug.Log("Player has reached the checkpoint " + CheckpointID);
        }
    }
}
