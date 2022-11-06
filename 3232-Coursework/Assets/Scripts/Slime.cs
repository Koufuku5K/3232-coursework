using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int life = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (life == 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // If bolt hits the slime's head, it instantly kills the slime
        if (col.otherCollider is CircleCollider2D)
        {
            Debug.Log("Headshot");
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
        // If bolt hits the slime's body, it decreases the slime's health by 1
        else if (col.otherCollider is BoxCollider2D)
        {
            Debug.Log("BodyShot");
            life--;
            Destroy(col.gameObject);
        }
    }
}
