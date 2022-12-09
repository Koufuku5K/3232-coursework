using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireBall : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;

    PlayerHUD playerHUD;
    CharacterAttributes player;
    EnemyAttributes enemy;
    GameOver gameOverScreen;

    public Animator animator;
    //public GameObject gameOverScreen;
    private Rigidbody2D rb;
    bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = FindObjectOfType<EnemyAttributes>();

        // Find the instance of Player prefab
        player = FindObjectOfType<CharacterAttributes>();

        // Find the PlayerHUD GameObject from the scene
        playerHUD = FindObjectOfType<PlayerHUD>();

        // Find the Game Over Screen GameObject from the scene
        gameOverScreen = FindObjectOfType<GameOver>();
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
            Debug.Log("Player Hit by: " + enemy.damage);
            // Set enemy damage back to normal
            enemy.damage = 10;

            // Update the health bar of the instance of the player
            playerHUD.HPSetup(player.currentHP);
        }
        else if (collider.tag == "Shield")
        {
            isColliding = true;
            StartCoroutine(showAnimation());
            // Set enemy damage back to normal
            enemy.damage = 10;
        }
    }
}
