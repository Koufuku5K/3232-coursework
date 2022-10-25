using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;
    
    EnemyHUD enemyHUD;
    CharacterAttributes player;
    EnemyAttributes enemy;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<CharacterAttributes>();
        
        // Find the instance of Enemy prefab
        enemy = FindObjectOfType<EnemyAttributes>();

        // Find the EnemyHUD GameObject from the scene
        enemyHUD = FindObjectOfType<EnemyHUD>();
    }

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
            // Damage the Enemy
            bool isDead = enemy.takeDamage(player.normalAttackDamage);
            Debug.Log("Enemy Hit!");

            // Update the health bar of the instance of the enemy
            enemyHUD.HPSetup(enemy.currentHP);
            Destroy(gameObject);

            if (isDead == true)
            {
                // TODO: Show End Screen
                Debug.Log("Enemy is Dead!");
            }
        }
    }
}
