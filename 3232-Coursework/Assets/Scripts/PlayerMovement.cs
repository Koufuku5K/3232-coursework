using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float mass = 0.01f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 force = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        animator.SetFloat("Horizontal", force.x);
        animator.SetFloat("Vertical", force.y);
        animator.SetFloat("Magnitude", force.magnitude);

        transform.position = transform.position + (force * (1 / mass)) * Time.deltaTime * Time.deltaTime;
    }
}
