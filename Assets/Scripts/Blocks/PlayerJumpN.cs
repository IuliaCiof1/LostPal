using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpN : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player;

    //Whenever this object is enabled/activated it will send some new position coordonates to the PlayerController
    //It will also enable movement
    public void OnEnable() 
    {
       
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<PlayerController>().Move = true;
        player.GetComponent<PlayerController>().TargetPosition = player.transform.position + new Vector3(0, 0.33f*2,0);
        //player.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
