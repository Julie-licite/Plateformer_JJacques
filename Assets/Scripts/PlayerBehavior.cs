using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{

    private PlayerInputs inputs;
    private Vector2 direction;

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
        inputs.Player.Move.performed += OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>();

    }

   

    // Fixed update pour modifier le rigidbody
    void FixedUpdate()
    {
        var myRigidBody = GetComponent<Rigidbody2D>();
        direction.y = 0;
        myRigidBody.MovePosition(direction);
    }
}
