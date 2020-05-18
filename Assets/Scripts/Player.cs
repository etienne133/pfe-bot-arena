using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    PlayerControls playerControls;
    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Gameplay.Fire.performed += ctx => Fire();
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
}
