using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveN : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player;

    //Whenever this object is enabled/activated it will send some new position coordonates to the PlayerController
    //It will also enable movement
    public void OnEnable() 
    {
        Debug.Log("move e");
        
        player.GetComponent<PlayerController>().Move = true;
        player.GetComponent<PlayerController>().TargetPosition = player.transform.position + new Vector3(0, 0.64f,0);
        gameObject.SetActive(false);
    }
}
