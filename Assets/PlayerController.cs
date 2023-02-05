using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody player;
    private float directionX = 0f;
    private float directionY = 0f;
    public float speed = 5f;
    public float walkUnits = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");
        //if (directionX > 0f)
        player.velocity = new Vector3(directionX * speed, player.velocity.y, directionY * speed);
        directionX = 0;
    }
}
