using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove1 : MonoBehaviour
{
    private CharacterController controller;
    public Joystick playerJoystick;
    public Joystick verticalJoystick;
    public GameObject playerBody;

    public float speed = 1;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
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

        controller.Move(move * speed * Time.deltaTime);
    }
}
