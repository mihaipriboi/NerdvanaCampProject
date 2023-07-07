using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private new BoxCollider2D collider2D;
    private SpriteRenderer sprite;
    private Animator anim;
    private Path path;
    private int currentWaypoint = 0;
    RaycastHit2D isGrounded;
    Seeker seeker;
    private bool isMovingRight = true;

    private enum MovementState { idle, running, damaged, death };

    private enum Behaviour { None, LeftToRight, LeftToRightSmart, Follow, SmartFollow };

    [SerializeField] private Behaviour behaviour;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [ShowIf(nameof(IsLeftToRight))][SerializeField] private Transform leftObs;
    [ShowIf(nameof(IsLeftToRight))][SerializeField] private Transform rightObs;

    [ShowIf(nameof(IsFollowBehaviour))][Header("Pathfinding")] public bool showPathfindingHeader = true;
    [ShowIf(nameof(IsFollowBehaviour))] public Transform target;

    [ShowIf(nameof(IsFollowBehaviour))] public float activateDistance = 50f;
    [ShowIf(nameof(IsFollowBehaviour))] public float pathUpdateSeconds = 0.5f;

    [ShowIf(nameof(IsFollowBehaviour))][Header("Physics")] public bool showPhysicsHeader = true;

    [ShowIf(nameof(IsFollowBehaviour))] public float nextWaypointDistance = 3f;
    [ShowIf(nameof(IsFollowBehaviour))] public float jumpNodeHeightRequirement = 0.8f;
    [ShowIf(nameof(IsFollowBehaviour))] public float jumpCheckOffset = 0.1f;
    [ShowIf(nameof(IsFollowBehaviour))] public float jumpModifier = 0.3f;


    [ShowIf(nameof(IsFollowBehaviour))][Header("Custom Behavior")] public bool showCustomBehaviorHeader = true;

    [ShowIf(nameof(IsFollowBehaviour))] public bool followEnabled = true;
    [ShowIf(nameof(IsFollowBehaviour))] public bool jumpEnabled = true;
    [ShowIf(nameof(IsFollowBehaviour))] public bool directionLookEnabled = true;

    [ShowIf(nameof(IsSmart))][SerializeField] private float smartLevel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        switch (behaviour)
        {
            case Behaviour.None:
                // Do nothing
                break;
            case Behaviour.LeftToRight:
                MoveLeftToRight();
                break;
            case Behaviour.LeftToRightSmart:
                MoveLeftToRightSmart();
                break;
            case Behaviour.Follow:
                // Do nothing
                break;
            case Behaviour.SmartFollow:
                if (TargetInDistance() && followEnabled)
                {
                    PathFollow();
                }
                break;
        }

        UpdateAnimationState();
    }

    private void MoveLeftToRight()
    {
        if (isMovingRight)
        {
            if (Math.Abs(transform.position.x - rightObs.position.x) < 1.0f) // Check if the enemy is too close to the obstacle
                isMovingRight = false;
            else
                rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            if (Math.Abs(transform.position.x - leftObs.position.x) < 1.0f)
                isMovingRight = true;
            else
                rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }


    private void MoveLeftToRightSmart()
    {
    }

    private void SmartFollowPlayer()
    {
    }

    private void UpdateAnimationState()
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
    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.5f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //jump
        if (jumpEnabled && isGrounded)
        {
            // Calculate Y difference
            float yDifference = Mathf.Abs(rb.position.y - path.vectorPath[currentWaypoint].y);

            // Only jump if y difference is above threshold
            if (yDifference > jumpNodeHeightRequirement)
            {
                // Calculate jump force based on y difference
                float jumpForce = yDifference * jumpModifier;
                rb.AddForce(Vector2.up * jumpForce);
            }
        }
        else
            rb.AddForce(force);


        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}