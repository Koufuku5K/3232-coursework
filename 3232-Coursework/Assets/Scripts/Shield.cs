using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Fireball")
        {
            Destroy(gameObject);
        }
    }
}
