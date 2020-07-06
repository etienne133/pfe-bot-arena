using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] BattleAgent theAgent;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] GameObject[] enemySpawnPoints;
    [SerializeField] float enemyColliderSize = 3; //Use for curriculum training
    // Start is called before the first frame update
    void Start()
    {
        if (!theAgent) {
            theAgent = GetComponent<BattleAgent>();
        }
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
        enemy.GetComponent<CapsuleCollider>().radius = 3;
    }

    public void enemyDead() {
        Debug.Log("Enemy killed");
        theAgent.RegisterKill();
        spawnEnemy();
    }

    public void projectileHitWall()
    {
        theAgent.projectileHitWall();
    }
}
