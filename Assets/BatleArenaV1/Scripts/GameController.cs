using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] BattleAgent theAgent;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] List<Enemy> enemyList;
    [SerializeField] GameObject[] enemySpawnPoints;
    [SerializeField] float enemyColliderSize = 3; //Use for curriculum training
    [SerializeField] string controllerName;
    [SerializeField] int numberOfObstacle;
    [SerializeField] List<IPlayer> playerList;
    [SerializeField] List<GameObject> ObstacleSpawnPoints;
    [SerializeField] GameObject ObstaclePrefab;
    private DataController dataController;
    // Start is called before the first frame update
    void Start()
    {
        playerList = new List<IPlayer>();

        enemyList = new List<Enemy>();
        if (!theAgent) {
            theAgent = GetComponent<BattleAgent>();
        }
        if (enemySpawnPoints.Length < 1) {
            Debug.LogError("No ennemi spawn points");
        }

        //initialize data controller
        dataController = this.gameObject.AddComponent<DataController>();
        List<int> playIDList = playerList.Select(player => player.PlayerID).ToList();
        dataController.initializePlayers(playIDList);
        spawnOBstacles();
        spawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnEnemy() {
        var spawnLocation = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
        // todo:face the character
        var enemy = Instantiate(enemyPrefab, spawnLocation.transform.position, Quaternion.Euler(0f, -90f, 0f));
        enemy.GetComponent<CapsuleCollider>().radius = enemyColliderSize;
        enemy.setGameController(this);
        enemyList.Add(enemy);
    }
    
    // Change to PlayerDead to be more general, for validation
    public void playerDead(int deadTeamID, int projectileTeamID) {
        Debug.Log("Enemy killed");

        // if trainingmode = true
        theAgent.RegisterKill();

        // will likely be removed
        spawnEnemy();
    }

    public void projectileHitWall()
    {
        theAgent.projectileHitWall();
    }

    public Vector3 distanceToEnemy() {
        if (!enemyList[0])
        {
            return new Vector3(10, 10, 10);
        }
        var agentPos = theAgent.transform.position;
        var enemyPos = enemyList[0].transform.position;
        return agentPos - enemyPos;

    }

    public void spawnOBstacles()
    {
        List<GameObject> selectedPoints = ObstacleSpawnPoints.OrderBy(x => Random.value * int.MaxValue).Take(numberOfObstacle).ToList();
        selectedPoints.ForEach(x => Debug.Log($"x: {x.transform.position.x} y: {x.transform.position.y}"));
        selectedPoints.ForEach(spawn =>
        {
            Instantiate(ObstaclePrefab, spawn.transform.position, Quaternion.Euler(0f, Random.value * 360, 0f));
        });

    }
}
