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

    public GameObject limitFrame;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    CharacterAttributes player;
    EnemyAttributes enemy;

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

        yield return new WaitForSeconds(1f);

        // Continue
        limitFrame.SetActive(false);
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
}
