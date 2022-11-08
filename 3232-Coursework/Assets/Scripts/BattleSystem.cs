using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State { START, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    // Reference to other Scripts
    public PlayerHUD playerHUD;
    public EnemyHUD enemyHUD;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject boltPrefab;
    public GameObject limitBoltPrefab;
    public GameObject fireBallPrefab;
    public GameObject shieldPrefab;

    public float gravity = -9.81f;
    public float time = 1.5f;
    public float impulseTime = 1.5f;
    public float limitBoltMass = 5f;

    public GameObject limitFrame;
    public GameObject finishItFrame;
    public GameObject pointer;
    public GameObject enemyBuffedText;
    public GameObject enemyRagedText;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    public Transform shieldSpawnPoint;
    public Transform boltSpawnPoint;
    public Transform limitBoltSpawnPoint;
    public Transform fireBallSpawnPoint;

    CharacterAttributes player;
    EnemyAttributes enemy;
    Bolt bolt;
    LimitBolt limitBolt;
    FireBall fireBall;

    public Button attackButton;
    public Button guardButton;
    public Button limitButton;

    public State battleState;

    private int enemyMove;

    // Start is called before the first frame update
    void Start()
    {
        // Find the instance of Enemy prefab
        enemy = FindObjectOfType<EnemyAttributes>();

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

        // If the enemy's health is 100 or less, show that it is raged
        if (enemy.currentHP <= 100)
        {
            enemyRagedText.SetActive(true);
        }

        // Enemy attack AI
        if (enemyHUD.waitSlider.value == enemyHUD.waitSlider.maxValue)
        {
            // If the previous move was a buff move, attack
            if (enemy.damage > 10)
            {
                enemyBuffedText.SetActive(false);
                enemyBasicAttack();
            }
            // If the enemy's health is 100 or less, buff basic attack damage
            else if (enemy.currentHP <= 100)
            {
                enemy.damage *= 2;
                enemyRandomAttack();
            }
            else
            {
                enemyRandomAttack();
            }
        }

        // Guide player to finish off the boss with limit
        if (playerHUD.limitSlider.value == playerHUD.limitSlider.maxValue && enemy.currentHP <= 50)
        {
            finishItFrame.SetActive(true);
            pointer.SetActive(true);

            // If player clicks limit or attack, remove the frame
            if (limitButton.GetComponent<Button>().enabled == false)
            {
                finishItFrame.SetActive(false);
                pointer.SetActive(false);
            }
            else if (attackButton.GetComponent<Button>().enabled == false)
            {
                finishItFrame.SetActive(false);
                pointer.SetActive(false);
            }
        }
        else
        {
            finishItFrame.SetActive(false);
            pointer.SetActive(false);
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

    // Player Moves

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

    // Enemy Moves
    public void enemyBasicAttack()
    {
        GameObject fireBallObject = Instantiate(fireBallPrefab, fireBallSpawnPoint);

        // Restart the wait slider to 0
        enemyHUD.waitSlider.value = 0;
    }

    // Buff next enemy damage
    public void enemyBuffAttack()
    {
        enemy.GetComponent<EnemyAttributes>().damage *= 2;
        enemyHUD.waitSlider.value = 0;
        enemyBuffedText.SetActive(true);
    }

    // Picks a random move (stochastic behaviour)
    public void enemyRandomAttack()
    {
        enemyMove = Random.Range(1, 3);
        Debug.Log("move pick: " + enemyMove);
        switch (enemyMove)
        {
            case 1:
                Debug.Log("Basic Attack!");
                enemyBasicAttack();
                break;
            case 2:
                Debug.Log("Buff Attack!");
                enemyBuffAttack();
                break;
        }
    }
}
