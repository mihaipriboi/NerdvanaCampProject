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

    private enum MovementState { idle, running, damaged, death };
    private enum Behaviour { None, LeftToRight, LeftToRightSmart, Follow, SmartFollow };
    //Left to Right is just a simple left-right follow
    //Left to Right Smart is more complex left-right with obstacle avoidance.
    //Follow is just a simple player follower thats gonna agro in a distace
    //Smart Follow is a more advance player follower with a better understading of the map

    [SerializeField] private Behaviour behaviour;

    //Normal
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    //Left to Right
    [ShowIf(nameof(IsLeftToRight))] [SerializeField] private Transform leftObs;
    [ShowIf(nameof(IsLeftToRight))] [SerializeField] private Transform rightObs;

    //Follow Player
    [ShowIf(nameof(IsFollowBehaviour))] [SerializeField] private Transform playerToFollow;
    [ShowIf(nameof(IsFollowBehaviour))] [SerializeField] private float agroDistance;

    //Smart Level
    [ShowIf(nameof(IsSmart))][SerializeField] private float smartLevel;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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

    private bool IsSmart()
    {
        return behaviour == Behaviour.SmartFollow || behaviour == Behaviour.LeftToRightSmart;
    }
}
