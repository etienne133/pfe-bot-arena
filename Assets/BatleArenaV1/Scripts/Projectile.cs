using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody Rigidbody;
    private Vector3 Direction;

    private GameController gameController;

    void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;

        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.velocity = Direction * 10f;
    }

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        //Invoke(nameof(SelfDestruct), 1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            gameController.projectileHitWall();
        }
    }
}