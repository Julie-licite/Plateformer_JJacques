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

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    private void OnEnable()
    {
        //recup la liste des inputs
        inputs = new PlayerInputs();
        inputs.Enable();
        //recuperer l'action "quand le joueur appui sur les boutons d'actions move"
        inputs.Player.Move.performed += OnMovePerformed;
        //recuperer l'action "quand le joueur n'appui pas sur les boutons d'actions move"
        inputs.Player.Move.canceled += OnMoveCanceled;

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
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
        

        direction.y = 0;
        //je limite ma vitesse max
        if (myRigidbody.velocity.sqrMagnitude < maxspeed )
            //j'ajoute une force de poussé a mon character
            myRigidbody.AddForce(direction * speed);

        //FLORE comment l'ordinateur traduit une valeur de float de la premiere  ligne en valeur bool de la condition du booleen
        //creer variable qui verifie si on cours
        var isRunning = direction.x != 0;
        //recuperer la condition de l'animator isRunning
        myAnimator.SetBool("isRunning", isRunning);

        //code pour flipper correctement le sprite dans la bonne direction en fonction de la valeur (vers où elle pointe)
        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;

        }
       else if (direction.x > 0)
        {
            myRenderer.flipX = false;
        }
    }
}
