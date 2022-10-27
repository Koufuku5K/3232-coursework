using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State { START, PLAYERMOVE, ENEMYMOVE, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    // Reference to other Scripts
    public PlayerHUD playerHUD;
    public EnemyHUD enemyHUD;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject boltPrefab;
    public GameObject limitBoltPrefab;
    public float launchForce = 1f;
    public float angle = 60;
    public float timeToHit = 1f;

    public GameObject limitFrame;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    public Transform climax;
    public Transform boltSpawnPoint;
    public Transform limitBoltSpawnPoint;

    CharacterAttributes player;
    EnemyAttributes enemy;
    Bolt bolt;
    LimitBolt limitBolt;

    public Button attackButton;
    public Button guardButton;
    public Button limitButton;

    public State battleState;

    // Start is called before the first frame update
    void Start()
    {
        battleState = State.START;
        BattleSetup();
    }

    void Update()
    {
        if (playerHUD.waitSlider.value == playerHUD.waitSlider.maxValue)
        {
            attackButton.GetComponent<Button>().enabled = true;
            guardButton.GetComponent<Button>().enabled = true;
        }
        else
        {
            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
        }
        
        if (playerHUD.limitSlider.value == playerHUD.limitSlider.maxValue)
        {
            limitButton.GetComponent<Button>().enabled = true;
        }
        else
        {
            limitButton.GetComponent<Button>().enabled = false;
        }

        if (enemyHUD.waitSlider.value == enemyHUD.waitSlider.maxValue)
        {
            enemyAttack();
        }
    }

    void BattleSetup()
    {
        //Spawn in Player on top of player spawn point
        GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint);
        player = playerObject.GetComponent<CharacterAttributes>();
        playerHUD.playerHUDSetup(player);

        //Spawn in Enemy on top of enemy spawn point
        GameObject enemyObject = Instantiate(enemyPrefab, enemySpawnPoint);
        enemy = enemyObject.GetComponent<EnemyAttributes>();
        enemyHUD.enemyHUDSetup(enemy);
    }

    public void Attack()
    {
        if (attackButton.GetComponent<Button>().enabled == true)
        {
            Debug.Log("Attack!");

            // Spawn the Bolt
            GameObject boltObject = Instantiate(boltPrefab, boltSpawnPoint);

            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
            limitButton.GetComponent<Button>().enabled = false;
            playerHUD.waitSlider.value = 0;
            Debug.Log("Attack Button Disabled");
            Debug.Log("Guard Button Disabled");
            Debug.Log("Limit Button Disabled");
        } 
        else
        {
            return;
        }
    }

    IEnumerator playerLimit()
    {
        // Show the Limit Frame
        if (limitFrame != null)
        {
            limitFrame.SetActive(true);
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Continue
        limitFrame.SetActive(false);

        GameObject limitBoltObject = Instantiate(limitBoltPrefab, limitBoltSpawnPoint);
        //Vector2 direction = climax.transform.position - limitBoltSpawnPoint.transform.position;
        var xDistance = enemySpawnPoint.transform.position.x - limitBoltSpawnPoint.transform.position.x;
        var initialVelocity = xDistance / Mathf.Cos(angle) * timeToHit;
        var velocityX = initialVelocity * Mathf.Cos(angle);
        var velocityY = initialVelocity * Mathf.Sin(angle);
        var climaxY = velocityY * timeToHit - (4.905f * timeToHit * timeToHit);
        var climaxX = velocityX * timeToHit;
        Vector2 climax = new Vector2(climaxX, climaxY);
        //limitBoltObject.GetComponent<Rigidbody2D>().
        //limitBoltObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
        limitBoltObject.GetComponent<Rigidbody2D>().AddForce(climax * launchForce);

        // Damage the Enemy
        bool isDead = enemy.takeDamage(player.limitDamage);

        enemyHUD.HPSetup(enemy.currentHP);
        
        if (isDead == true)
        {
            // TODO: Show End Screen
            Debug.Log("Enemy is Dead!");
        }
    }

    IEnumerator playerGuard()
    {
        // Immune to damage
        player.immune = true;

        if (player.immune == true)
        {
            Debug.Log("Player is Immune!");
            enemy.damage = 0;
        }

        // Wait for seconds
        yield return new WaitForSeconds(player.waitMax);

        // Continue
        Debug.Log("Player is no longer Immune!");
        player.immune = false;
        enemy.damage = 10;
    }

    public void Limit()
    {
        if (limitButton.GetComponent<Button>().enabled == true)
        {
            StartCoroutine(playerLimit());
            Debug.Log("Limit!");
            limitButton.GetComponent<Button>().enabled = false;
            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
            playerHUD.limitSlider.value = 0;
            Debug.Log("Attack Button Disabled");
            Debug.Log("Guard Button Disabled");
            Debug.Log("Limit Button Disabled");
        }
        else
        {
            return;
        }
    }

    public void Guard()
    {
        if (guardButton.GetComponent<Button>().enabled == true)
        {
            StartCoroutine(playerGuard());
            Debug.Log("Guard!");
            guardButton.GetComponent<Button>().enabled = false;
            attackButton.GetComponent<Button>().enabled = false;
            limitButton.GetComponent<Button>().enabled = false;
            playerHUD.waitSlider.value = 0;
            Debug.Log("Guard Button Disabled");
            Debug.Log("Attack Button Disabled");
            Debug.Log("Limit Button Disabled");
        }
    }

    public void enemyAttack()
    {
        // Damage the Player
        bool isDead = player.takeDamage(enemy.damage);

        playerHUD.HPSetup(player.currentHP);

        // Restart the wait slider to 0
        enemyHUD.waitSlider.value = 0;

        if (isDead == true)
        {
            // TODO: Show End Screen
            Debug.Log("Player is Dead!");
        }
    }
}
