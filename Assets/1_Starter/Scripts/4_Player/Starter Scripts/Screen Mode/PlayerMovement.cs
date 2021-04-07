using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Moves player on keyboard, with or without character controller

    [Header("Player Speed")]
    public float speed = 2;

    [Header("To use transforms instead of character controller-driven motion")]
    public bool noPhysics;

    private CharacterController controller;


    void Start()
    {
        if(!noPhysics) //If Character Controller method is being used
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        float x = Input.GetAxis("Horizontal"); //Gets left/right input
        float z = Input.GetAxis("Vertical"); //Gets forward/backward input

        float y = 0f;

        if (Input.GetKey(KeyCode.R)) //Keyboard hack for up/down
        {
            y = 1f;
        }
        else
        if (Input.GetKey(KeyCode.F))
        {
            y = -1f;
        }

        Vector3 move = transform.right * x + transform.up*y + transform.forward * z; //Creates motion vector

        if(!noPhysics) //If using character controller inbuilt functions
        {
            controller.Move(move * speed * Time.deltaTime);
        }
        else //Directly translate otherwise
        {
            this.transform.Translate(move * speed * Time.deltaTime);
        }

    }
}
