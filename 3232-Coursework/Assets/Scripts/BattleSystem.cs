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
    public GameObject shieldPrefab;

    public float gravity = -9.81f;
    public float time = 1.5f;
    public float impulseTime = 1.5f;
    public float limitBoltMass = 5f;

    public GameObject limitFrame;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    public Transform shieldSpawnPoint;
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

        // Launch the limit bolt
        var forceX = (((enemySpawnPoint.position.x - limitBoltObject.transform.position.x) / time) * limitBoltMass) / impulseTime;
        var forceY = (((enemySpawnPoint.position.x - limitBoltObject.transform.position.x) / time - (0.5 * gravity * time)) * limitBoltMass) / impulseTime;
        Vector2 force = new Vector2((float)forceX, (float)forceY);
        limitBoltObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    IEnumerator playerGuard()
    {
        GameObject shieldObject = Instantiate(shieldPrefab, shieldSpawnPoint);
        Debug.Log("Player Shielded!");

        // Wait for seconds
        yield return new WaitForSeconds(5f);

        // Continue
        Debug.Log("Player is no longer Immune!");
        Destroy(shieldObject);
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
