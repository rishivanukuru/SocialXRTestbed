using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class XRModeManager : MonoBehaviour
{
    public static int xrMode;
    //Initial Camera
    public GameObject initialCamera;

    //Base Canvas
    //Initial Panel
    public GameObject initialPanel;

    //Basic Controls
    public GameObject basicControlsPanel;

    //AR Objects
    public GameObject arObjects;
    public GameObject arSession;

    //Screen Objects
    public GameObject screenObjects;

    public GameObject generalInterfaceControls;


    // Start is called before the first frame update
    void Start()
    {
        xrMode = 0;
        initialCamera.SetActive(true);
        initialPanel.SetActive(true);
        screenObjects.SetActive(false);
        arObjects.SetActive(false);
        arSession.SetActive(false);
        generalInterfaceControls.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenMode()
    {
        xrMode = 2;
        initialPanel.SetActive(false);
        initialCamera.SetActive(false);

        basicControlsPanel.SetActive(true);

        screenObjects.SetActive(true);
        generalInterfaceControls.SetActive(true);

    }

    public void ARMode()
    {
        xrMode = 1;
        initialPanel.SetActive(false);
        initialCamera.SetActive(false);

        basicControlsPanel.SetActive(true);

        arObjects.SetActive(true);
        arSession.SetActive(true);
        generalInterfaceControls.SetActive(true);

    }

}
