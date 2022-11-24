using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    private enum State
    {
        Chasing,
        Attacking
    }

    private enum Direction
    {
        Right,
        Left
    }

    private State state;
    public Animator animator;

    Vector2 currentPos;
    Vector2 lastPos;

    Transform target;
    private NavMeshAgent agent;

    MobSpawner mobSpawner;

    void Awake()
    {
        state = State.Chasing;
    }

    void Start()
    {
        // Get the target game object from parent game object
        mobSpawner = GetComponentInParent<MobSpawner>();
        // Get the range collider from the child game object
        CircleCollider2D range = GetComponentInChildren<CircleCollider2D>();
        target = mobSpawner.target.transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        // Check if player is in enemy range
        checkRange();

        switch (state)
        {
            default:
            case State.Chasing:
                chasePlayer();
                break;
            case State.Attacking:
                attackPlayer();
                chasePlayer();
                break;
        }

        // Check if the slime is going right or left and flip sprite horizontally accordingly
        lastPos = currentPos;
        currentPos = transform.position;
        checkDirection();

        agent.SetDestination(target.position);
    }

    public void chasePlayer()
    {
        // Chase the player
        agent.SetDestination(target.position);
    }

    // Check the direction of the slime. If negative,
    // make the slime look left. If positive, make the slime look right.
    void checkDirection()
    {
        if (currentPos.x > lastPos.x)
        {
            transform.localScale = new Vector3(3f, 3f, 3f);
        }
        else if (currentPos.x < lastPos.x)
        {
            transform.localScale = new Vector3(-3f, 3f, 3f);
        }
        else
        {
            // do nothing
        }
    }

    void attackPlayer()
    {
        animator.SetBool("isAttacking", true);
    }

    // If player is in enemy range, change state to attacking. Else, change state to chasing.
    void checkRange()
    {
        if (Mathf.Abs(transform.position.x - target.position.x) <= 2 &&
            Mathf.Abs(transform.position.y - target.position.y) <= 2)
        {
            state = State.Attacking;
        }
        else
        {
            state = State.Chasing;
            animator.SetBool("isAttacking", false);
        }
    }
}