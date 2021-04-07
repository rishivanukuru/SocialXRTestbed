using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove1 : MonoBehaviour
{
    //Moves player using joysticks (for phone 2D mode)

    //See joystick script for more info (from asset store)
    [Header("Joysticks")]
    public Joystick playerJoystick;
    public Joystick verticalJoystick;
    [Header("Player Data")]
    public GameObject playerBody;
    public float speed = 1;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>(); //Link character controller
    }

    private void Update()
    {
        //Generate motion vector
        float x = playerJoystick.Horizontal;
        float z = playerJoystick.Vertical;
        float y;

        if (verticalJoystick)
        {
            y = verticalJoystick.Vertical;
        }
        else
        {
            y = 0f;
        }

        Vector3 move = playerBody.transform.right * x + playerBody.transform.forward * z + playerBody.transform.up*y;

        //Move character
        controller.Move(move * speed * Time.deltaTime);
    }
}
