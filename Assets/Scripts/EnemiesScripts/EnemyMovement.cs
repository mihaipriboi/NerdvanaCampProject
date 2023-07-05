using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public class EnemyMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private enum MovementState { idle, running, damaged, attack, death };
    private enum Behaviour { None, LeftToRight, LeftToRightSmart, Follow, SmartFollow };
    [SerializeField] private Behaviour behaviour;

    //Normal
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    //Left to Right
    [ShowIf("IsLeftToRight")] [SerializeField] private Transform leftObs;
    [ShowIf("IsLeftToRight")] [SerializeField] private Transform rightObs;

    //Follow Player
    [ShowIf("IsFollowBehaviour")] [SerializeField] private Transform playerToFollow;
    [ShowIf("IsFollowBehaviour")] [SerializeField] private float agroDistance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsFollowBehaviour()
    {
        return behaviour == Behaviour.Follow || behaviour == Behaviour.SmartFollow;
    }

    private bool IsLeftToRight()
    {
        return behaviour == Behaviour.LeftToRight || behaviour == Behaviour.LeftToRightSmart;
    }
}
