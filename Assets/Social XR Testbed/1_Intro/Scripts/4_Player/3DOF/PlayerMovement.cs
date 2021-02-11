using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    public float speed = 2;

    public bool noPhysics;

    void Start()
    {
        if(!noPhysics)
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float y = 0f;

        if (Input.GetKey(KeyCode.R))
        {
            y = 1f;
        }
        else
        if (Input.GetKey(KeyCode.F))
        {
            y = -1f;
        }

        Vector3 move = transform.right * x + transform.up*y + transform.forward * z;

        if(!noPhysics)
        {
            controller.Move(move * speed * Time.deltaTime);
        }
        else
        {
            this.transform.Translate(move * speed * Time.deltaTime);
        }

        


    }
}
