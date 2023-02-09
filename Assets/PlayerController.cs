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
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
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
            //If player moves to the right
            if (TargetPosition.y == transform.position.y && TargetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
                moveEW = true;
                moveN = false;
                moveS = false;
            }

            //If player moves to the left
            if (TargetPosition.y == transform.position.y && TargetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
                moveEW = true;
                moveN = false;
                moveS = false;
            }
            
            //If player moves up
            if (TargetPosition.x == transform.position.x && TargetPosition.y > transform.position.y)
            {
                moveEW = false;
                moveN = true;
                moveS = false;
            }
            
            //If player moves to the down
            if (TargetPosition.x == transform.position.x && TargetPosition.y < transform.position.y)
            {
                moveEW = false;
                moveN = false;
                moveS = true;
            }


            Vector3 newPosition = Vector3.MoveTowards(playerRb.position, TargetPosition, speed * Time.deltaTime); 
            playerRb.MovePosition(newPosition);

            // if (Vector2.Distance(player.position, targetPosition) <= 0.1f)
            if (playerRb.position == TargetPosition)
            { 
                Move = false;
                moveEW = false;
                moveN = false;
                moveS = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }
}
