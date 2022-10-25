using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;

    public EnemyHUD enemyHUD;

    public Rigidbody2D rb;

    CharacterAttributes player;
    EnemyAttributes enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<CharacterAttributes>();
        enemy = enemyObject.GetComponent<EnemyAttributes>();
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
            /**
            // Damage the Enemy
            bool isDead = enemy.takeDamage(player.normalAttackDamage);
            Debug.Log("Enemy Hit!");

            // enemyHUD.HPSetup(enemy.currentHP);

            if (isDead == true)
            {
                // TODO: Show End Screen
                Debug.Log("Enemy is Dead!");
            }
            */
        }
    }
}
