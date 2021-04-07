using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenModeManager : MonoBehaviour
{

    //Manages the choice between the non-AR modes of PC and Mobile

    [Header("PC Scripts")]
    public MouseLook playerMouse;
    public PlayerMovement playerMovement;

    [Header("Mobile Scripts")]
    public GyroCamera playerGyro;
    public JoystickMove1 playerJoystick;

    void Awake()
    {
        //Disable all scripts
        playerMouse.enabled = false;
        playerMovement.enabled = false;

        playerGyro.enabled = false;
        playerJoystick.enabled = false;
        
    }

   
    public void SetPCMode()
    {
        playerMouse.enabled = true;
        playerMovement.enabled = true;
    }

    public void SetMobileMode()
    {
        playerGyro.enabled = true;
        playerJoystick.enabled = true;

        playerGyro.CalibrateFunction(); //Calibrate gyro once
    }

    

}
