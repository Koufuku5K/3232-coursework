using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBolt : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;

    EnemyHUD enemyHUD;
    CharacterAttributes player;
    EnemyAttributes enemy;

    public Animator animator;
    private Rigidbody2D rb;
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
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (isColliding == true)
        {
            rb.velocity = new Vector2(0f, 0f);
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
        if (collider.tag == "Enemy")
        {
            isColliding = true;
            StartCoroutine(showAnimation());
            // Damage the Enemy
            bool isDead = enemy.takeDamage(player.limitDamage);
            Debug.Log("Enemy Hit!");

            // Update the health bar of the instance of the enemy
            enemyHUD.HPSetup(enemy.currentHP);

            if (isDead == true)
            {
                Debug.Log("Enemy is Dead!");
            }
        }
    }
}
