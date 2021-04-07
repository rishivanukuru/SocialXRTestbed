using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //Control player look direction using the mouse

    [Header("Mouse Data")]
    public float mouseSensitivity = 100;
    [Header("Body to Rotate")]
    public Transform playerBody;

    private float xRotation = 0;
    private float yRotation = 0;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; //In case you want to lock the cursor, which you shouldn't, because you won't be able to use any of the UI then
    }

    void Update()
    {
        //Generate movement values
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotate Head and Body along Y
        transform.Rotate(Vector3.up * mouseX);
        playerBody.Rotate(Vector3.up * mouseX);

        //Calculate Up/Down rotation
        xRotation -= mouseY;

        //Clamp to avoid gimbal lock
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //Apply X rotation to head
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

    }
}
