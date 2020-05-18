using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    int speed = 10;
    PlayerControls playerControls;
    Vector2 move;
    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Gameplay.Fire.performed += ctx => Fire();
        playerControls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        playerControls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
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
        // vertical axis is Y so move on X and Z.
        Vector3 moveVector = new Vector3(move.x, 0, move.y) * Time.deltaTime * speed;
        transform.Translate(moveVector, Space.Self);
    }
}
