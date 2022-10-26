using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBolt : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = new Vector2(10f, 5f);
        //float angle = Mathf.Atan2(rb.velocity.y = 5f, rb.velocity.x = 10f) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
