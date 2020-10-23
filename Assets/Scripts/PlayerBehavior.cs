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
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask floor;

    private PlayerInputs inputs;
    private Vector2 direction;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    private bool isOnFloor = false;
    private bool footOnFloor = false;

    private void OnEnable()
    {
        //recup la liste des inputs
        inputs = new PlayerInputs();
        inputs.Enable();
        //recuperer l'action "quand le joueur appui sur les boutons d'actions move"
        inputs.Player.Move.performed += OnMovePerformed;
        //recuperer l'action "quand le joueur n'appui pas sur les boutons d'actions move"
        inputs.Player.Move.canceled += OnMoveCanceled;
        //recuperer l'action jump dans le menu des inputs
        inputs.Player.Jump.performed += JumpOnPerformed;

        //recup mon rigidbody et lui donne le nom d'une variable + facile a utiliser
        myRigidbody = GetComponent<Rigidbody2D>();
        //recup mon animator et lui donne le nom d'une variable + facile a utiliser
        myAnimator = GetComponent<Animator>();
        //recup mon sprite renderer et lui donne le nom d'une variable + facile a utiliser
        myRenderer = GetComponent<SpriteRenderer>();
    }



    //lorsque l'action jump est effectuée :
    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        //j'active l'animation de saut de manière pas très clean mais fonctionne quand même
        myAnimator.SetBool("isJumping", true);

        
        //SI le joueur touche le sol ça autorise le joueur a sauter
        if (isOnFloor)
        {
            //j'ajoute de la force soudaine (avec la vitesse specifié dans le serializefield a mon rigidbody quand jump est activé
            myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //je signale que quand je quitte donne de l'imppulsion forcement je suis plus sur le sol et je ne peux plus sauter
            isOnFloor = false;
        }


    }

    //lorsque les touches directions sont utilisée
    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        //je lis les valeurs produites par mes touches
        direction = obj.ReadValue<Vector2>();

    }

    //quand les touches directions sont pas touché, le player ne bouge pas
    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        //mes valeurs sont automatiquement a zéro
        direction = Vector2.zero;

    }

    

    // Fixed update pour modifier le rigidbody plus precisement (repeté + de fois que l'update
    void FixedUpdate()
    {
      
        direction.y = 0;

        //je limite ma vitesse max
        if (myRigidbody.velocity.sqrMagnitude < maxspeed)
        {
            //j'ajoute une force de poussé a mon character
            myRigidbody.AddForce(direction * speed);
        }

         
        //creer variable qui verifie si on cours
        var isRunning = direction.x != 0;
        //recuperer la condition de l'animator isRunning
        myAnimator.SetBool("isRunning", isRunning);

        //code pour flipper correctement le sprite dans la bonne direction en fonction de la valeur (vers où elle pointe)
        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;

        }

        // pour être sur que le sprite va pas rester fixer dans le mauvais sens
        else if (direction.x > 0)
        {
            myRenderer.flipX = false;
        }

        //setup Raycast pour verifier si le joueur a une surface SOUS lui
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        //si on detecte un truc en dessous alors ...
        if (hit.collider != null)
        {
           
            //on a bien quelque chose sous les pieds de droit et on peut sauté
            footOnFloor = true;
       
        }

    }



    //detecter la collision avec le sol en utilisant des layers (meme si jai moins peur des tags)
     void OnCollisionEnter2D(Collision2D other)
     {
        //variable qui verifie si on touche bien un bloc de sol (mais tout ce qui fais parti des plateformes physiques)
        var touchFloor = floor == (floor | (1 << other.gameObject.layer));
        
        //si on touche une surface comprise dans le layer floor ET que le raycast detecte une surface droite sous les pieds alors ...
        if (touchFloor && footOnFloor )
        {

            //on est bien sur le sol (et on peut sauter (ref au JumpOnPerformed))
            isOnFloor = true;
            
        }

        //mon animation de saut est negative
        myAnimator.SetBool("isJumping", false);

     }

   


}
