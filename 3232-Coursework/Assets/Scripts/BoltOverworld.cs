using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltOverworld : MonoBehaviour
{
    public Rigidbody2D rb;

    Vector2 currentVelocity;

    bool isColliding = false;
    public int colCount = 0;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = rb.velocity;

        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (isColliding == true)
        {
            rb.velocity = new Vector2(0f, 0f);
        }

        if (colCount == 3)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            var speed = currentVelocity.magnitude;
            var newDir = Vector2.Reflect(currentVelocity.normalized, col.contacts[0].normal);

            rb.velocity = newDir * Mathf.Max(speed, 0f);

            colCount += 1;
        }
        else if (col.gameObject.tag == "WorldBorder")
        {
            Destroy(gameObject);
        }
    }
}
