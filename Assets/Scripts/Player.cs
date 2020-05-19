using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    int moveSpeed = 10, turnSpeed = 10;
    PlayerControls playerControls;
    Vector2 move;
    Vector2 aim;
    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Gameplay.Fire.performed += ctx => Fire();

        playerControls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        playerControls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        playerControls.Gameplay.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
        playerControls.Gameplay.Aim.canceled += ctx => aim = Vector2.zero;

    }

    public void Fire()
    {
        Debug.Log("Fire!!");
    }

    void OnEnable()
    {
        playerControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }

    private void Update()
    {
        // move
        // vertical axis is Y so move on X and Z.
        Vector3 moveVector = new Vector3(move.x, 0, move.y) * Time.deltaTime * moveSpeed;
        transform.Translate(moveVector, Space.Self);

        // aim
        //Vector3 mPos = new Vector3(aim.x, 0 , aim.y);
        //transform.Rotate(r, Space.Self);

        //Vector2 lookDir = mPos - transform.position;
        //float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f;
        //Quaternion rotationQ = new Quaternion();
        //rotationQ.SetFromToRotation(transform.position, mPos);
        //transform.rotation = rotationQ * transform.rotation;
    }
}
