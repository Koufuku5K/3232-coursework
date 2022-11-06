using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int life = 2;
    public SpriteRenderer sprite;

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

    public IEnumerator slimeDamaged()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // If the bolt hits the slime's head, it instantly kills the slime
        if (col.otherCollider is CircleCollider2D)
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
        // If the bolt hits the slime's body, it decreases the slime's health by 1
        else if (col.otherCollider is BoxCollider2D)
        {
            StartCoroutine(slimeDamaged());
            life--;
            Destroy(col.gameObject);
        }
    }
}
