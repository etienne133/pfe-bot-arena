using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
/*
 Based on https://github.com/Sebastian-Schuchmann/A.I.-Shooting-Game-ML-Agents-Unity-Example for now.
 Replace progressively with our own stuff.
*/
public class BattleAgent: Agent
{
    public int score = 0;
    public float speed = 3f;
    public float rotationSpeed = 3f;

    public Transform shootingPoint;
    public int minStepsBetweenShots = 50;
    public int damage = 100;

    public Projectile projectile;
    public GameController gameController;

    private bool ShotAvaliable = true;
    private int StepsUntilShotIsAvaliable = 0;

    private Vector3 StartingPosition;
    private Rigidbody Rb;
    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    private float nextFire;
    private Boolean canFire;
    public float fireRate;

    void Update()
    {
        if (canFire && Time.time > nextFire)
        {
            //Cooldown
            nextFire = Time.time + fireRate;

            //Spawn
            var spawnedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.Euler(0f, -90f, 0f));
            spawnedProjectile.SetDirection(transform.forward);

            canFire = false;
            //Audiosound if wanted
            //GetComponent<AudioSource>().Play();
        }
    }
    private void Shoot()
    {
        canFire = true;
        //if (!ShotAvaliable)
        //    return;

        //var layerMask = 1 << LayerMask.NameToLayer("Enemy");
        //var direction = transform.forward;

        //var spawnedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.Euler(0f, -90f, 0f));
        //spawnedProjectile.SetDirection(direction);

        // We'll use layer mask when we need to see through walls!!

        //Debug.DrawRay(transform.position, direction, Color.blue, 1f);
        //if (Physics.Raycast(shootingPoint.position, direction, out var hit, 200f, layerMask))
        //{
        //    //hit.transform.GetComponent<Enemy>().GetShot(damage, this);
        //}
        //else
        //{
        //    AddReward(-0.033f);
        //}

        //ShotAvaliable = false;
        //StepsUntilShotIsAvaliable = minStepsBetweenShots;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // "can shoot"
        sensor.AddObservation(ShotAvaliable);

        //consider it's own position.
        sensor.AddObservation(this.transform.position);
    }

    private void FixedUpdate()
    {
        // Can be changed to take less decisions if we want
        RequestDecision();

        if (!ShotAvaliable)
        {
            StepsUntilShotIsAvaliable--;

            if (StepsUntilShotIsAvaliable <= 0)
                ShotAvaliable = true;
        }

        AddReward(-1f / MaxStep);
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.RoundToInt(vectorAction[0]) >= 1)
        {
            canFire = true;
            //Shoot();
        }
        else
        {
            canFire = false;
        }
        //Modulate for Max speed! Do here to prevent ai from cheating...
        Vector3 movementVector = new Vector3(vectorAction[1], 0f, vectorAction[2]);
        movementVector = Vector3.ClampMagnitude(movementVector, 1f);

        Rb.velocity = new Vector3(movementVector.x * speed, 0f, movementVector.z * speed);
        //Rb.AddForce(movementVector * speed);
        transform.Rotate(Vector3.up, vectorAction[3] * rotationSpeed);

        //direct movements
        //if (vectorAction[1] <= -0.5f)
        //{
        //    transform.position += Time.deltaTime * speed * Vector3.left;
        //}
        //else if (vectorAction[1] >= 0.5f)
        //{
        //    transform.position += Time.deltaTime * speed * Vector3.right;
        //}

        //if (vectorAction[2] <= -0.5f)
        //{
        //    transform.position += Time.deltaTime * speed * Vector3.back;
        //}
        //else if (vectorAction[2] >= 0.5f)
        //{
        //    transform.position += Time.deltaTime * speed * Vector3.forward;
        //}
    }

    public override void Initialize()
    {
        StartingPosition = transform.position;
        Rb = GetComponent<Rigidbody>();

        //TODO: Delete
        Rb.freezeRotation = true;
        EnvironmentParameters = Academy.Instance.EnvironmentParameters;
    }

    /* Controls: WASD to move, Q and R to turn, Space to shoot */
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetKey(KeyCode.Space) ? 1f : 0f;

        //Side momentum
        actionsOut[1] = Input.GetAxis("Horizontal");

        //Forward momentum
        actionsOut[2] = Input.GetAxis("Vertical");

        //rotation
        float rotation = 0f;
        if (Input.GetKey(KeyCode.Q)) rotation = -1f;
        else if(Input.GetKey(KeyCode.E)) rotation = 1f;
        actionsOut[3] = rotation;


        // New movement
    }

    public override void OnEpisodeBegin()
    {
        //not sure about this?
        OnEnvironmentReset?.Invoke();

        //Load Parameter from Curciulum
        minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequenzy", 30f));

        //set starting point
        transform.position = StartingPosition;
        Rb.velocity = Vector3.zero;
        ShotAvaliable = true;
    }

    public void RegisterKill()
    {
        //AddReward(1.0f / EnvironmentParameters.GetWithDefault("amountZombies", 4f));
        //TODO: Environment parameters with config
        AddReward(1.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.CompareTag("enemy"))
        //{
        //    //enemyManager.SetEnemiesActive();
        //    AddReward(-1f);
        //    EndEpisode();
        //}

        // If agent gets touched by a projectile, die.
        //if (other.gameObject.CompareTag("projectile"))
        //{
        //    Debug.Log("Dead");
        //    AddReward(-1f);
        //    EndEpisode();
        //}
    }

}
