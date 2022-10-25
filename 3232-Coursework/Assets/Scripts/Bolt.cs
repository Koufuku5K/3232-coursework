using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public Rigidbody2D rb;

    CharacterAttributes player;
    EnemyAttributes enemy;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(20f, 0f);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            // Damage Enemy
            Debug.Log("Enemy Hit!");
            // Destroy the bolt
            Destroy(gameObject);
        }
    }
}
