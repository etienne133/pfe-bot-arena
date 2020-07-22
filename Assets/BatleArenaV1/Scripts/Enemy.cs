using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] public int teamID = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();

            if (projectile.TeamID != this.teamID)
            {
                gameController.playerDead(this.teamID, projectile.TeamID);

                //Destroy the projectile
                Destroy(collision.gameObject);

                //Destroy the enemy
                Destroy(gameObject);

            }

            ////Destroy the projectile
            //Destroy(collision.gameObject);

            ////Destroy the enemy
            //Destroy(gameObject);

            //gameController.enemyDead();
        }
    }

    public void setGameController(GameController gc) {
        gameController = gc;
    }
}
