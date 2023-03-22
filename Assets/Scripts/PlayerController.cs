using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    [SerializeField] private float speed = 7f;
    [SerializeField] private Animator animator;

    public bool Move { get; set; }
    public Vector2 TargetPosition { get; set; }

    private bool moveN, moveS, moveEW;

    //[SerializeField] private GameObject finishPanel;
    public bool IsWin { get; set; }
    
    public delegate void PlayerWins();
    public static event PlayerWins OnPlayerWins;
    
    public delegate void PlayerFails();
    public static event PlayerWins OnPlayerFails;

    private AudioSource audioSource;

    private Vector2 startPosition;
    private Vector3 newPosition;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(IsWin)
            OnPlayerWins?.Invoke();
    }
    
    // Update is called once per frame
    public void FixedUpdate()
    {
        
        //Movement animation
         animator.SetBool("moveEW",moveEW);
         animator.SetBool("moveN",moveN);
         animator.SetBool("moveS",moveS);
        
         if (Move)
         {
             audioSource.enabled = true;//PlayOneShot(audioClip);
             //If player moves to the right
             if (TargetPosition.y == transform.position.y && TargetPosition.x > transform.position.x)
             {
                 transform.localScale = new Vector2(-1, 1);
                 moveEW = true;
                 moveN = false;
                 moveS = false;
             }
        
             //If player moves to the left
             else if (TargetPosition.y == transform.position.y && TargetPosition.x < transform.position.x)
             {
                 transform.localScale = new Vector2(1, 1);
                 moveEW = true;
                 moveN = false;
                 moveS = false;
             }
             
             //If player moves up
             else if (TargetPosition.x == transform.position.x && TargetPosition.y > transform.position.y)
             {
                 moveEW = false;
                 moveN = true;
                 moveS = false;
             }
             
             //If player moves to the down
             else if (TargetPosition.x == transform.position.x && TargetPosition.y < transform.position.y)
             {
                 moveEW = false;
                 moveN = false;
                 moveS = true;
             }

             newPosition = Vector3.MoveTowards(playerRb.position, TargetPosition, speed * Time.deltaTime); 
             playerRb.MovePosition(newPosition);
             
             if (playerRb.position == TargetPosition)
             { 
                 Move = false;
                 moveEW = false;
                 moveN = false;
                 moveS = false;
             }
         }
         else
         {
             audioSource.enabled = false;
         }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Finish"))
        {
            IsWin = true;
        }
    }

    public void RestartPosition()
    {
        TargetPosition = startPosition;
        transform.position = startPosition;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
       //TargetPosition = transform.position;
        RestartPosition();
        OnPlayerFails?.Invoke();
    }
}
