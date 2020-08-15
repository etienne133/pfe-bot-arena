using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Transactions;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class BattleAgent: Agent, IPlayer
{
    //[SerializeField] public int teamID = 0;
    //[SerializeField] public int playerID = 0;

    BehaviorParameters agentBehaviorParam;
    
    public int PlayerID { get; set; }
    public int TeamID { get; set; }

    public int score = 0;
    public float speed = 3f;
    public float rotationSpeed = 3f;

    public Transform shootingPoint;
    public int minStepsBetweenShots = 50;
    public int damage = 100;

    public Projectile projectile;
    public GameController gameController { get; set; }

    private bool ShotAvaliable = true;
    private int StepsUntilShotIsAvaliable = 0;

    private Vector3 StartingPosition;
    private Rigidbody Rb;
    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    private float nextFire;
    private Boolean canFire;
    public float fireRate;

    void Start()
    {
        //PlayerID = playerID;
        //TeamID = teamID;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        //BehaviorParameters myBehaviorParam = gameObject.GetComponent<BehaviorParameters>();
        //myBehaviorParam.TeamId = TeamID;
    }
    void Update()
    {
        if (canFire && Time.time > nextFire)
        {
            //Cooldown
            nextFire = Time.time + fireRate;

            //Spawn
            var spawnedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.Euler(0f, -90f, 0f));
            spawnedProjectile.SetDirection(transform.forward);
            spawnedProjectile.GetComponent<Projectile>().TeamID = this.TeamID;
            spawnedProjectile.GetComponent<Projectile>().PlayerID = this.PlayerID;

            canFire = false;
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

        // Consider distance to ennemies
        var distances = gameController.distanceToEnemies(PlayerID);
        distances.ForEach(distVector => sensor.AddObservation(distVector.magnitude));
        
    }

    private void FixedUpdate()
    {
        // Can be changed to take less decisions if we want
        RequestDecision();

        //limit shots.
        if (!ShotAvaliable)
        {
            StepsUntilShotIsAvaliable--;

            if (StepsUntilShotIsAvaliable <= 0)
                ShotAvaliable = true;
        }

        AddReward(-1f / this.MaxStep);

        // Reward based on distance to ennemy
        var distances = gameController.distanceToEnemies(PlayerID);


        float minDist = float.MaxValue;
        distances.ForEach(distVector => {
            if (distVector.magnitude < minDist)
                minDist = distVector.magnitude;
            //AddReward(0.01f / distVector.magnitude);
            //if (distVector.magnitude <= 8)
            //{
            //    AddReward(0.1f / distVector.magnitude);
            //}
        });
        AddReward(0.01f / minDist);

        //gameController.distanceToEnemies(PlayerID).ForEach(distVector => {
        //    // Give reward for proximity
        //    if (distVector.magnitude <= 8f)
        //    {
        //        AddReward(0.1f);
        //    }
        //});
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
        transform.Rotate(Vector3.up, vectorAction[3] * rotationSpeed);

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
        //minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequenzy", 30f));

        //set starting point
        transform.position = StartingPosition;
        Rb.velocity = Vector3.zero;
        ShotAvaliable = true;
    }

    public void RegisterKill()
    {
        //AddReward(1.0f / EnvironmentParameters.GetWithDefault("amountZombies", 4f));
        //TODO: Environment parameters with config
        Debug.Log("kill registered");
        AddReward(1.0f);
        EndEpisode();
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
        if (other.gameObject.CompareTag("projectile"))
        {   
            Projectile projectile = other.gameObject.GetComponent<Projectile>();

            if (projectile.TeamID != this.TeamID)
            {
                Debug.Log("Dead");

                // Put a check if(trainMode){}
                AddReward(-1f);
                EndEpisode();
                //----------------------------
            }
        }
    }

    public void projectileHitWall()
    {
        AddReward(-0.05f);
    }
}
