using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 playerPos;
    private Vector2 slimeMovement;
    public float moveSpeed = 1f;
    private bool isAttacking = false;
    public GameObject Range;
    public Rigidbody2D rb;

    Vector2 currentPos;
    Vector2 lastPos;

    Vector3 localVelocity;

    public Animator animator;

    void Awake()
    {
        state = State.Chasing;
        rb = this.GetComponent<Rigidbody2D>();
        localVelocity = transform.InverseTransformDirection(Vector3.forward);
    }

    // Start is called before the first frame update
    void Start()
    {
        CircleCollider2D range = GetComponentInChildren<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void chasePlayer()
    {
        // Calculate how far the slime is from the player
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 direction = playerPos - transform.position;
        direction.Normalize();
        slimeMovement = direction;
        moveSlime(slimeMovement);
    }

    public void moveSlime(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

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
        Debug.Log("Attack");
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            state = State.Attacking;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        state = State.Chasing;
        animator.SetBool("isAttacking", false);
        Debug.Log("No Longer Attacking");
    }
}