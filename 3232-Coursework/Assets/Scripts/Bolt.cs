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

    public Animator animator;

    bool isColliding = false;

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
        if (isColliding == true)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        else
        {
            rb.velocity = new Vector2(10f, 0f);
        }
    }

    IEnumerator showAnimation()
    {
        animator.SetBool("isHit", true);
        yield return new WaitForSeconds(1f);
        isColliding = false;
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            isColliding = true;
            StartCoroutine(showAnimation());
            // Damage the Enemy
            bool isDead = enemy.takeDamage(player.normalAttackDamage);
            Debug.Log("Enemy Hit!");

            // Update the health bar of the instance of the enemy
            enemyHUD.HPSetup(enemy.currentHP);

            if (isDead == true)
            {
                // TODO: Show End Screen
                Debug.Log("Enemy is Dead!");
            }
        }
    }
}
