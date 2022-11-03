using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBolt : MonoBehaviour
{
    public Transform boltSpawnPoint;
    public Transform crosshair;
    public Rigidbody2D rb;
    public float shootForce;

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
        var aimX = crosshair.position.x - boltSpawnPoint.position.x;
        var aimY = crosshair.position.y - boltSpawnPoint.position.y;
        Vector2 aimDirection = new Vector2((float) aimX, (float) aimY);

        // Make the bolt speed the same no matter how far the crosshair position is from the player
        aimDirection.Normalize();
        GameObject bolt = Instantiate(boltOverworldPrefab, boltSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bolt.GetComponent<Rigidbody2D>();
        rb.velocity = aimDirection * shootForce;
    }
}
