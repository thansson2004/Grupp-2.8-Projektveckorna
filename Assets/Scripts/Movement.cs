using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Ian
public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = .01f;  

    

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal"); //Gör så att man kan röra sig upp och ner
        float yDirection = Input.GetAxis("Vertical"); // Gör så att man kan röra sig höger och vänster

        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f); // Gör så att man kan röra sig

        transform.position += moveDirection * speed; // Räknar ut om hur snabbt man går åt de hållet man går

        // ᏇᎩፈᎤᏖᏂåᏒ
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



