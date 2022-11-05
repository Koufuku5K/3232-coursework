using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManipulation : MonoBehaviour
{
    public GameObject player;
    bool isColliding = false;

    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = true;

            if (isColliding == true)
            {
                // Change player mass when they are inside the gravity hole.
                playerMovement.mass += 0.015f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // Change player mass back to normal once they are outside of the gravity hole.
        isColliding = false;
        playerMovement.mass = 0.01f;
    }
}
