using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class XRModeHandler : MonoBehaviour
{
    public static int xrMode;

    [Header("Initial Scene Camera")]
    public GameObject initialCamera;

    [Header("AR Mode Choice UI")]
    public GameObject initialPanel;

    //AR Objects
    [Header("AR Objects")]
    public GameObject arObjects;
    public GameObject arSession;

    //Screen Objects
    [Header("Screen Objects")]
    public GameObject screenObjects;
    public GameObject joystickMove;
    public GameObject joystickJump;

    [Header("Main UI Controls")]
    public GameObject mainUIControls;


    // Start is called before the first frame update
    void Start()
    {
        xrMode = 0;

        //Set mode choice screen active
        initialCamera.SetActive(true);
        initialPanel.SetActive(true);

        //Disable mode objects
        screenObjects.SetActive(false);
        arObjects.SetActive(false);
        arSession.SetActive(false);
        mainUIControls.SetActive(false);
    }

    public void ARMode()
    {
        xrMode = 1;

        //Deactivate Mode Panel
        initialPanel.SetActive(false);
        initialCamera.SetActive(false);

        //Activate AR objects
        arObjects.SetActive(true);
        arSession.SetActive(true);

        //Activate main UI
        mainUIControls.SetActive(true);

    }

    public void ScreenMode()
    {
        xrMode = 2;

        //Deactivate Mode Panel
        initialPanel.SetActive(false);
        initialCamera.SetActive(false);

        //Activate Screen Objects
        screenObjects.SetActive(true);

        //Activate Main UI
        mainUIControls.SetActive(true);

        //Disable Joysticks in PC mode
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (joystickMove != null)
                joystickMove.SetActive(false);

            if (joystickJump != null)
                joystickJump.SetActive(false);
        }

    }

    
}
