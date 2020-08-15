using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
public class GameController : MonoBehaviour
{

    //[SerializeField] BattleAgent theAgent;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] GameObject AIPlayerPrefab;
    //[SerializeField] List<Enemy> enemyList;
    //[SerializeField] GameObject[] enemySpawnPoints;
    [SerializeField] GameObject[] PlayerSpawnLocation;
    [SerializeField] int numberOfAIPlayer= 4;
    [SerializeField] float enemyColliderSize = 3; //Use for curriculum training
    //[SerializeField] string controllerName;
    [SerializeField] int numberOfObstacle = 10;
    //[SerializeField] List<IPlayer> playerList;
    [SerializeField] List<GameObject> ObstacleSpawnPoints;
    [SerializeField] GameObject ObstaclePrefab;
    [SerializeField] Boolean isTrainingModeOn = true;
    [SerializeField] int numberOfValidationGames = 1;

    private List<GameObject> spawnedObstacles;
    private DataController dataController;
    private int gamesPlayed = 0;
    private Dictionary<int,BattleAgent> battleAgentDictionary;
    // Start is called before the first frame update
    void Start()
    {
        //playerList = new List<IPlayer>();
        battleAgentDictionary = new Dictionary<int, BattleAgent>();
        //enemyList = new List<Enemy>();
        //if (!theAgent) {
        //    theAgent = GetComponent<BattleAgent>();
        //}
        //if (enemySpawnPoints.Length < 1) {
        //    Debug.LogError("No ennemi spawn points");
        //}

        //initialize data controller
        dataController = this.gameObject.AddComponent<DataController>();
        //List<int> playIDList = playerList.Select(player => player.PlayerID).ToList();

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void spawnEnemy() {
    //    var spawnLocation = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)];
    //    // todo:face the character
    //    var enemy = Instantiate(enemyPrefab, spawnLocation.transform.position, Quaternion.Euler(0f, -90f, 0f));
    //    enemy.GetComponent<CapsuleCollider>().radius = enemyColliderSize;
    //    enemy.setGameController(this);
    //    enemyList.Add(enemy);
    //}
    
    // Change to PlayerDead to be more general, for validation
    public void playerDead(int deadTeamID, int projectileTeamID) {
        Debug.Log("Enemy killed");

        // if trainingmode = true
        if (isTrainingModeOn)
        {
            battleAgentDictionary[projectileTeamID].RegisterKill();
        }


        //TODO: check if game done.


        // will likely be removed. respawn player?
        //spawnEnemy();
    }

    public void projectileHitWall(int playerID)
    {

        //theAgent.projectileHitWall();
        battleAgentDictionary[playerID].projectileHitWall();
    }

    public List<Vector3> distanceToEnemies(int playerID) {
        //if (!enemyList[0])
        //{
        //    return new Vector3(10, 10, 10);
        //}
        //var agentPos = theAgent.transform.position;
        //var enemyPos = enemyList[0].transform.position;
        //return age
        BattleAgent ba = battleAgentDictionary[playerID];
        List<Vector3> enemies = battleAgentDictionary.Values.Where(x=> x.PlayerID != playerID).Select(x => x.transform.position - ba.transform.position).ToList();
        return enemies;

    }

    public void spawnOBstacles()
    {
        List<GameObject> selectedPoints = ObstacleSpawnPoints.OrderBy(x => UnityEngine.Random.value * int.MaxValue).Take(numberOfObstacle).ToList();

        selectedPoints.ForEach(spawn =>
        {
            //Spawn obstacle and keep reference in list
            spawnedObstacles.Add(Instantiate(ObstaclePrefab, spawn.transform.position+new Vector3(0,0.2f,0), Quaternion.Euler(0f, UnityEngine.Random.value * 360, 0f)));
        });

    }

    public void StartGame()
    {
        spawnedObstacles = new List<GameObject>();
        spawnOBstacles();
        spawnAIPlayers();
        //spawnEnemy();
        if(gamesPlayed == 0)
        {
            List<int> playerIDs = battleAgentDictionary.Keys.ToList();
            if (dataController)
            {
                playerIDs.ForEach(x => Debug.Log(x));
                dataController.initializePlayers(playerIDs);
            }
        }

        if (isTrainingModeOn)
        {
            //Do MLAgent init stuff
        }
    }

    public void EndGame()
    {
        gamesPlayed++;
        //cleanUp Obstacles
        spawnedObstacles.ForEach(x => Destroy(x));

        if (!isTrainingModeOn && gamesPlayed < numberOfValidationGames) {
            StartGame();
        } 
    }

    public void spawnAIPlayers()
    {
        for(int i = 0; i < numberOfAIPlayer; i++)
        {
            BattleAgent player = Instantiate(AIPlayerPrefab, PlayerSpawnLocation[i].transform.position, Quaternion.Euler(0f, -90f, 0f)).GetComponent<BattleAgent>();
            player.gameController = this;
            player.PlayerID = i;
            player.TeamID = i;
            player.GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().TeamId=i;
            battleAgentDictionary.Add(i, player);
        }
    }
}
