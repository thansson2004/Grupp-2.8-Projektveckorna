using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = .01f;

    

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);

        transform.position += moveDirection * speed;

        animator.SetFloat("Horizontal", xDirection);
        animator.SetFloat("Vertical", yDirection);
        
        animator.SetFloat("Speed", moveDirection.sqrMagnitude);


 
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }



    public LayerMask interactableLayer;
    Vector2 movement;
    public Animator animator;








}



