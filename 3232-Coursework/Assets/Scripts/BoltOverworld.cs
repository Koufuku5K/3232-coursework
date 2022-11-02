using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltOverworld : MonoBehaviour
{
    //public Transform boltSpawnPoint;
    public Transform crosshair;
    public Rigidbody2D rb;

    public float boltForce;

    bool isColliding = false;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (isColliding == true)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }
}
