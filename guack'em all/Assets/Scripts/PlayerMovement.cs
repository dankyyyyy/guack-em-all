using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed;

    private Rigidbody2D rb;

    private float x;
    private float y; 

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(x * movespeed, y * movespeed);
    }

    private void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    } 
}
