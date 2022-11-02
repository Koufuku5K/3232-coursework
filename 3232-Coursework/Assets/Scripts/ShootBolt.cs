using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBolt : MonoBehaviour
{
    public Transform boltSpawnPoint;
    public Transform crosshair;
    public Rigidbody2D rb;

    public GameObject boltOverworldPrefab;

    public float boltForce;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var testX = crosshair.position.x - boltSpawnPoint.position.x;
        var testY = crosshair.position.y - boltSpawnPoint.position.y;

        GameObject bolt = Instantiate(boltOverworldPrefab, boltSpawnPoint.position, boltSpawnPoint.rotation);
        Rigidbody2D rb = bolt.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(testX, testY);
    }
}
