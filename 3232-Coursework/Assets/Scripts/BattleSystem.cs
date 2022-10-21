using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State { START, PLAYERMOVE, ENEMYMOVE, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    // Reference to other Scripts
    public BattleHUD battleHUD;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    CharacterAttributes player;
    CharacterAttributes enemy;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Button attackButton;
    public Button guardButton;

    public State battleState;

    // Start is called before the first frame update
    void Start()
    {
        battleState = State.START;
        BattleSetup();
    }

    void Update()
    {
        if (battleHUD.waitSlider.value == battleHUD.waitSlider.maxValue)
        {
            attackButton.GetComponent<Button>().enabled = true;
            guardButton.GetComponent<Button>().enabled = true;
        }
        else
        {
            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
        }
    }

    void BattleSetup()
    {
        //Spawn in Player on top of player spawn point
        GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint);
        player = playerObject.GetComponent<CharacterAttributes>();
        playerHUD.HUDSetup(player);

        //Spawn in Enemy on top of enemy spawn point
        GameObject enemyObject = Instantiate(enemyPrefab, enemySpawnPoint);
        enemy = enemyObject.GetComponent<CharacterAttributes>();
        enemyHUD.HUDSetup(enemy);
    }

    public void Attack()
    {
        if (attackButton.GetComponent<Button>().enabled = true)
        {
            Debug.Log("Attack!");
            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
            battleHUD.waitSlider.value = 0;
            Debug.Log("Attack Button Disabled");
            Debug.Log("Guard Button Disabled");
        } 
        else
        {
            return;
        }
    }

    public void Guard()
    {
        if (guardButton.GetComponent<Button>().enabled = true)
        {
            Debug.Log("Guard!");
            guardButton.GetComponent<Button>().enabled = false;
            attackButton.GetComponent<Button>().enabled = false;
            battleHUD.waitSlider.value = 0;
            Debug.Log("Guard Button Disabled");
            Debug.Log("Attack Button Disabled");
        }
    }
}
