using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;

    PlayerHUD playerHUD;
    CharacterAttributes player;
    EnemyAttributes enemy;

    public Animator animator;
    private Rigidbody2D rb;
    bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = enemyObject.GetComponent<EnemyAttributes>();

        // Find the instance of Player prefab
        player = FindObjectOfType<CharacterAttributes>();

        // Find the PlayerHUD GameObject from the scene
        playerHUD = FindObjectOfType<PlayerHUD>();
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
            rb.velocity = new Vector2(-20f, -7f);
        }
    }

    IEnumerator showAnimation()
    {
        animator.SetBool("isHit", true);
        Debug.Log("isHit True");
        yield return new WaitForSeconds(1f);
        isColliding = false;
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = true;
            StartCoroutine(showAnimation());
            // Damage the Player
            bool isDead = player.takeDamage(enemy.damage);
            Debug.Log("Player Hit!");

            // Update the health bar of the instance of the player
            playerHUD.HPSetup(player.currentHP);

            if (isDead == true)
            {
                // TODO: Show End Screen
                Debug.Log("Enemy is Dead!");
            }
        }
        else if (collider.tag == "Shield")
        {
            isColliding = true;
            StartCoroutine(showAnimation());
        }
    }
}
