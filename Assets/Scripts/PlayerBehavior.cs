using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxspeed;
    private PlayerInputs inputs;
    private Vector2 direction;

    private void OnEnable()
    {
        //recup la liste des inputs
        inputs = new PlayerInputs();
        inputs.Enable();
        //recuperer l'action "quand le joueur appui sur les boutons d'actions move"
        inputs.Player.Move.performed += OnMovePerformed;
        //recuperer l'action "quand le joueur n'appui pas sur les boutons d'actions move"
        inputs.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        direction = Vector2.zero;

    }

    //lorsque le joueur appui donc on produit cette valeure
    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>();

    }

    


    // Fixed update pour modifier le rigidbody plus precisement (repeté + de fois que l'update
    void FixedUpdate()
    {
        //je recupère le rigidbody de mon character
        var myRigidBody = GetComponent<Rigidbody2D>();

        direction.y = 0;
        //je limite ma vitesse max
        if (myRigidBody.velocity.sqrMagnitude < maxspeed )
            //j'ajoute une force de poussé a mon character
            myRigidBody.AddForce(direction * speed);

    }
}
