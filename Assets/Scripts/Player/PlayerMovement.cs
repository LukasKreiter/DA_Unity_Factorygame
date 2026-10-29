using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
 
    public float speed = 12f;
    public float maxFlyHeight = 10f;
    public float minFlyHeight = 0f;
    
 
    
 
    Vector3 velocity;
 
 
    
    void Update()
    {
        
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
 
        
        Vector3 move = transform.right * x + transform.forward * z;

        float y = 0f;

        //Makes you able to fly (Drone)
        if (Input.GetKey(KeyCode.Space))
        {
            y=1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            y=-1f;
        }

        Vector3 velocity = (move + Vector3.up * y) * speed;
        controller.Move(velocity * Time.deltaTime);
 
        //Set Max Height limit for flying
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minFlyHeight, maxFlyHeight);
        transform.position= pos;
 
       
    }
}