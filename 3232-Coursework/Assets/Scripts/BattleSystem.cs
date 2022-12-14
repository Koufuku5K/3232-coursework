using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.Linq;
using MinMaxLibrary.algorithms;
using MinMaxLibrary.utils;
using System;
using MinMaxProjects.tests;

public enum State { START, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{

    enum HumanPlayerActions
    {
        Attack,
        Guard,
        NOOP
    }

    MinMaxLibrary.utils.NTree<MinMaxLibrary.algorithms.MinMax<string, MinMaxProjects.tests.TreeBasedGameConfiguration<string>, MinMaxProjects.tests.TreeBasedPlayerConf>.CurrentGameState> truncTree, prevTree;
    Func<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState, string, Optional<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>> f;
    
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
    public GameObject youDiedFrame;

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
    AudioManager am;

    MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf> conf;

    public Button attackButton;
    public Button guardButton;
    public Button limitButton;

    public State battleState;
    HumanPlayerActions ha = HumanPlayerActions.NOOP;

    private int enemyMove;

    /// <summary>
    /// DISCLAIMER: 
    /// This script is not created by me. Refer to README for the reference.
    /// </summary>
    void fitTree()
    {

        // The player can perform two different actions:
        // - Attack: Attacks opponent by 10 damage
        // - Guard:  Nullifies opponent damage
        HashSet<string> actionsPlayer = new HashSet<string>();
        actionsPlayer.Add("PlayerAttack");
        actionsPlayer.Add("Guard");

        // The boss can perform two actions
        HashSet<string> actionsBoss = new HashSet<string>();
        actionsBoss.Add("BossAttack");
        actionsBoss.Add("Buff");

        /// Initialization of the minmax algorithm
        var cgs = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState();
        /// Setting some parameters out of the constructor, so to provide a better comment section...
        var initConf = new TreeBasedGameConfiguration<string>(); // initial state of the tree game
        cgs.gameConfiguration = initConf;                // initial board configuration
        cgs.opponentLifeBar = new TreeBasedPlayerConf(((float)enemy.currentHP)/200.0, true);
        cgs.playerLifeBar = new TreeBasedPlayerConf(((float)player.currentHP) / 100.0, true);
        cgs.isPlayerTurn = false;                        // starting with max
        cgs.parentAction = "";                           // the root node has NO parent action, represented as an empty string!\

        /// In a more realistic setting, we do not care if we reached the final state or not, let the algorithm decide
        /// upon the score of each single player! in here, I only set the damage for each player.
        f = (conff, act) =>
        {

            if (((conff.opponentLifeBar.getScore()) < 0.009) || ((conff.playerLifeBar.getScore()) < 0.009))
            {
                return new Optional<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>();
            }

            // Creating a new configuration, where we change the turn
            var result = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState(conff, act);
            // Appending the action in the history
            result.gameConfiguration = conff.gameConfiguration.appendAction(act);
            result.parentAction = act;
            // Setting up the actions by performing some damage
            if (conff.isPlayerTurn)
            {
                if (act.Equals("PlayerAttack"))
                    result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - 0.1, 0.0), true);
                else
                    result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - 0.0, 0.0), true);
            }
            else
            {
                if (act.Equals("BossAttack"))
                    result.playerLifeBar = new TreeBasedPlayerConf(Math.Max(result.playerLifeBar.getScore() - 0.1, 0.0), true);
                else
                    result.playerLifeBar = new TreeBasedPlayerConf(Math.Max(result.playerLifeBar.getScore() - 0.0, 0.0), true);
            }
            return result;
        };

        conf = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>(actionsBoss, actionsPlayer, f);

        /// Running the whole MinMax algorithm with alpha/beta pruning
        Console.WriteLine("Tree with Min/Max approach and Alpha/Beta Pruning:");
        Console.WriteLine("==================================================");
        truncTree = conf.fitWithAlphaBetaPruning(cgs);
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        // Find the instance of Enemy prefab
        enemy = FindObjectOfType<EnemyAttributes>();
        am = FindObjectOfType<AudioManager>();

        battleState = State.START;
        BattleSetup();

        fitTree();
    }

    void Update()
    {
        playerButtons();

        updateState();
        switch (battleState)
        {
            case State.LOSE:
                SceneManager.LoadScene("GameOver");
                break;
            case State.WIN:
                SceneManager.LoadScene("YouWin");
                break;
        }

        enemyAI();

        suggestLimit();
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

    // Game Feedback Codes

    // Disable buttons if slider is not full, enable otherwise
    public void playerButtons()
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

    // Suggest Player to finish off the boss with limit move
    public void suggestLimit()
    {
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

    /// <summary>
    /// DISCLAIMER: 
    /// This script is not created by me. Refer to README for the reference.
    /// </summary>
    public void updateTree(string element)
    {
        if ((truncTree == null) || (truncTree.getChildrenSize() == 0))
            fitTree();
        // Navigating the tree data structure
        if (truncTree.data.isPruned)
        {
            truncTree = conf.fitWithAlphaBetaPruning(truncTree.data);
        }
        prevTree = truncTree;
        truncTree = conf.navigateTreeWithAction(truncTree, element);
        if (truncTree == null)
        {
            // If I reached a pruned node, then it means that I need to re-calculate the algorithm
            truncTree = conf.fitWithAlphaBetaPruning(prevTree.data);
            truncTree = conf.navigateTreeWithAction(truncTree, element);
        }
        if ((truncTree == null) || (truncTree.getChildrenSize() == 0))
            fitTree();
        if (truncTree.data.isPruned)
        {
            // If I reached a pruned node, then it means that I need to re-calculate the algorithm
            //truncTree = conf.fitWithAlphaBetaPruning(truncTree.data);
            truncTree = conf.fitWithAlphaBetaPruning(prevTree.data);
            truncTree = conf.navigateTreeWithAction(truncTree, element);
        }
    }

    // Player Codes

    /// <summary>
    /// Attack function for the player. Clicking on the attack button will spawn an instance
    /// of a bolt prefab which deals damage to the boss
    /// </summary>
    public void Attack()
    {
        if (attackButton.GetComponent<Button>().enabled == true)
        {
            if ((truncTree == null) || (truncTree.getChildrenSize() == 0))
                fitTree();
            if (!truncTree.data.isPlayerTurn)
            {
                //updateTree("Buff");
                updateTree("BossAttack");
            }
            updateTree("PlayerAttack");
            UnityEngine.Debug.Log(truncTree.data.action.getBestAction());

            UnityEngine.Debug.Log("Attack!");

            // Spawn the Bolt
            GameObject boltObject = Instantiate(boltPrefab, boltSpawnPoint);

            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
            limitButton.GetComponent<Button>().enabled = false;
            playerHUD.waitSlider.value = 0;
            UnityEngine.Debug.Log("Attack Button Disabled");
            UnityEngine.Debug.Log("Guard Button Disabled");
            UnityEngine.Debug.Log("Limit Button Disabled");
            ha = HumanPlayerActions.Attack;
        }
        else
        {
            ha = HumanPlayerActions.NOOP;
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

    /// <summary>
    /// Limit function for the player. Clicking on the limit button will deal a huge damage to
    /// the boss.
    /// </summary>
    public void Limit()
    {
        if (limitButton.GetComponent<Button>().enabled == true)
        {
            StartCoroutine(playerLimit());
            UnityEngine.Debug.Log("Limit!");
            limitButton.GetComponent<Button>().enabled = false;
            attackButton.GetComponent<Button>().enabled = false;
            guardButton.GetComponent<Button>().enabled = false;
            playerHUD.limitSlider.value = 0;
            UnityEngine.Debug.Log("Attack Button Disabled");
            UnityEngine.Debug.Log("Guard Button Disabled");
            UnityEngine.Debug.Log("Limit Button Disabled");
        }
        else
        {
            return;
        }
    }

    IEnumerator playerGuard()
    {
        GameObject shieldObject = Instantiate(shieldPrefab, shieldSpawnPoint);
        UnityEngine.Debug.Log("Player Shielded!");

        // Wait for seconds
        yield return new WaitForSeconds(5f);

        // Continue
        UnityEngine.Debug.Log("Player is no longer Immune!");
        Destroy(shieldObject);
    }

    /// <summary>
    /// Guard function for the player. Clicking on the guard button will nullify the boss' attack.
    /// </summary>
    public void Guard()
    {
        if (guardButton.GetComponent<Button>().enabled == true)
        {
            StartCoroutine(playerGuard());

            if ((truncTree == null) || (truncTree.getChildrenSize() == 0))
                fitTree();
            if (!truncTree.data.isPlayerTurn)
            {
                updateTree("Buff");
                UnityEngine.Debug.Log(truncTree.data.action.getBestAction());
                //enemyBuffAttack();
            }
            updateTree("Guard");
            //UnityEngine.Debug.Log(truncTree.data.action.getBestAction());

            UnityEngine.Debug.Log("Guard!");
            guardButton.GetComponent<Button>().enabled = false;
            attackButton.GetComponent<Button>().enabled = false;
            limitButton.GetComponent<Button>().enabled = false;
            playerHUD.waitSlider.value = 0;
            UnityEngine.Debug.Log("Guard Button Disabled");
            UnityEngine.Debug.Log("Attack Button Disabled");
            UnityEngine.Debug.Log("Limit Button Disabled");
            ha = HumanPlayerActions.Guard;
        } else
        {
            ha = HumanPlayerActions.NOOP;
            return;
        }
    }

    // Enemy Codes

    /// <summary>
    /// Boss' basic attack that deals 10 damage to the player.
    /// </summary>
    public void enemyBasicAttack()
    { 
        GameObject fireBallObject = Instantiate(fireBallPrefab, fireBallSpawnPoint);

        // Restart the wait slider to 0
        enemyHUD.waitSlider.value = 0;
    }

    /// <summary>
    /// This function buffs the next boss attack damage by 100%
    /// </summary>
    public void enemyBuffAttack()
    {
        enemy.GetComponent<EnemyAttributes>().damage *= 2;
        enemyHUD.waitSlider.value = 0;
        enemyBuffedText.SetActive(true);
    }

    /// <summary>
    /// Function that uses the tree to determine the best action for the boss.
    /// </summary>
    public void enemyAttack()
    {
        enemyMove = UnityEngine.Random.Range(1, 3);
        UnityEngine.Debug.Log("move pick: " + enemyMove);

        if (truncTree == null)
        {
            // If I reached a pruned node, then it means that I need to re-calculate the algorithm
            truncTree = conf.fitWithAlphaBetaPruning(prevTree.data);
            truncTree = conf.navigateTreeWithAction(truncTree, prevTree.data.action.getBestAction());
        }

        if ((truncTree == null) || (truncTree.getChildrenSize() == 0))
        {
            fitTree();
        }

        if (truncTree.data.isPruned)
        {
            // If I reached a pruned node, then it means that I need to re-calculate the algorithm
            //truncTree = conf.fitWithAlphaBetaPruning(truncTree.data);
            truncTree = conf.fitWithAlphaBetaPruning(prevTree.data);
            truncTree = conf.navigateTreeWithAction(truncTree,  prevTree.data.action.getBestAction());
        }

        if (ha == HumanPlayerActions.Guard)
        {
            enemyBuffAttack();
        }
        else if (truncTree.data.action.getBestAction().Equals("BossAttack"))
        {
            enemyBasicAttack();
        } 
        updateTree(truncTree.data.action.getBestAction());
    }

    /// <summary>
    /// Enemy AI. When the boss's health is less than or equal to half, the boss will
    /// receive the RAGED status. This increases the enemy's basic attack move by 100%
    /// </summary>
    public void enemyAI()
    {
        // If the enemy's health is 100 or less, show that it is raged
        if (enemy.currentHP <= 100)
        {
            enemyRagedText.SetActive(true);
        }

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
                enemyAttack();
            }
            else
            {
                enemyAttack();
            }
        }
    }

    /// <summary>
    /// Checks if the player or the boss is dead
    /// </summary>
    public void updateState()
    {
        if (player.currentHP == 0)
        {
            battleState = State.LOSE;
        }
        else if (enemyHUD.hpSlider.value == 0)
        {
            battleState = State.WIN;
        }
    }
}
