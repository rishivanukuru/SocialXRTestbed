using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    public float speed = 12;

    public float heightControl = 0.001f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetKey(KeyCode.R))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + heightControl * Time.deltaTime, transform.position.z);
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y-heightControl*Time.deltaTime, transform.position.z);
        }


    }
}
