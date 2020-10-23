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

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }




    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {

        myAnimator.SetBool("isJumping", true);
        Debug.Log("ciel");
        //si le joueur touche le sol ça autorise le joueur a sauter
        if (isOnFloor)
        {
            //j'ajoute de la force soudaine (avec la vitesse specifié dans le serializefield a mon rigidbody quand jump est activé
            myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //je signale que quand je quitte donne de l'imppulsion forcement je suis plus sur le sol
            isOnFloor = false;
        }


    }

    //quand les touches directions sont pas touché, le player ne bouge pas
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
        if (myRigidbody.velocity.sqrMagnitude < maxspeed)
        {
            //j'ajoute une force de poussé a mon character
            myRigidbody.AddForce(direction * speed);
        }

           

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

        else if (direction.x > 0)
        {
            myRenderer.flipX = false;
        }

        //setup Raycast pour verifier si le joueur a une surface SOUS lui

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        if (hit.collider != null)
        {
            Debug.Log("footonfloor");
            footOnFloor = true;
            
        }




    }

    //detecter la collision avec le sol en utilisant des layers (meme si jai moins peur des tags)
     void OnCollisionEnter2D(Collision2D other)
    {
        var touchFloor = floor == (floor | (1 << other.gameObject.layer));
        var touchFromAbove = other.contacts[0].normal == Vector2.up;


        if (touchFloor && /*touchFromAbove*/ footOnFloor )
        {
            isOnFloor = true;
            
        }

        if (touchFromAbove)
        {
            Debug.Log("fjdshfsehfujeshfse");
        }

        myAnimator.SetBool("isJumping", false);




    }

   


}
