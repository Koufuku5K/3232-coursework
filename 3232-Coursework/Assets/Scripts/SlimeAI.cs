using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    private enum State
    {
        Chasing,
        Attacking,
    }

    private State state;

    public Vector3 playerPos;
    private Vector2 slimeMovement;
    public float moveSpeed = 1f;
    private Rigidbody2D rb;

    void Awake()
    {
        state = State.Chasing;
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    void FixedUpdate()
    {
        //moveSlime(slimeMovement);
    }

    public void chasePlayer()
    {
        // Calculate how far the slime is from the player
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 direction = playerPos - transform.position;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;*/
        direction.Normalize();
        slimeMovement = direction;
        moveSlime(slimeMovement);
    }

    public void moveSlime(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
