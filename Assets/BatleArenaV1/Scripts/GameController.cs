using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] BattleAgent theAgent;
    // Start is called before the first frame update
    void Start()
    {
        if (!theAgent) {
            theAgent = GetComponent<BattleAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enemyDead() {
        Debug.Log("Enemy killed");
        theAgent.RegisterKill();
    }

}
