using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
using UnityEngine.UI;

public class XRModeOptions : MonoBehaviour
{

    //Script that determines device capabilities and presents modes accordingly

    [Header("AR Foundation Prefab")]
    public GameObject ARSessionPrefab;
    private GameObject ARSessionObj;

    [Header("Mode Buttons")]
    public GameObject roomScaleButton;
    public GameObject pcModeButton;
    public GameObject arCoreInstallButton;

    [Header("UI Text")]
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI messageText;

    private bool isARSupported;
    private bool triedInstall = false;

    bool isGyro = false;
    bool isNextStepDone = false;
    bool arCheck = false;

    private void Start()
    {

        if(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)   //Allow PC mode in editor and on Windows
        {
            pcModeButton.SetActive(true);
        }
        else
        {
            pcModeButton.SetActive(false);
        }

        messageText.SetText("Please wait...");  //Initial text while AR Check takes place

        //Deactivate Buttons temporarily
        roomScaleButton.GetComponent<Button>().interactable = false; 
        arCoreInstallButton.SetActive(false);

        //Set check flag to be false
        isNextStepDone = false;

        //Begin AR Check
        StartCoroutine(CheckAvailability());
        arCheck = true;

        //Pause the instantiated AR Session from starting before the AR check takes place
        ARSessionPrefab.GetComponent<ARSession>().attemptUpdate = false;
    }

    private IEnumerator CheckAvailability()
    {
        instructionText.SetText("Checking Availability...");

        if (!ARSessionObj)
        {
            ARSessionObj = Instantiate(ARSessionPrefab);
            yield return new WaitForSecondsRealtime(0.5f);
        }
        if (!(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)) //As long as it's not on PC
        {
            Debug.Log("ARSession.state: " + ARSession.state);
            switch (ARSession.state)
            {
                case ARSessionState.CheckingAvailability:
                    Debug.Log("Still Checking Availability...");
                    instructionText.SetText("Still Checking AR Availability...");
                    ARSession.stateChanged += ARSessionStateChanged;
                    break;
                case ARSessionState.NeedsInstall:
                    Debug.Log("Supported, not installed, requesting installation");
                    instructionText.SetText("AR Supported, not installed, requesting installation");
                    InstallMode();
                    break;
                case ARSessionState.Installing:
                    Debug.Log("Supported, apk installing");
                    instructionText.SetText("AR Supported, apk installing");
                    InstallMode();
                    break;
                case ARSessionState.Ready:
                    Debug.Log("Supported and installed");
                    instructionText.SetText("AR Supported and installed");
                    NextStep(true);
                    break;
                case ARSessionState.SessionInitializing:
                    Debug.Log("Supported, apk installed. SessionInitializing...");
                    instructionText.SetText("AR Supported, apk installed.");
                    NextStep(true);
                    break;
                case ARSessionState.SessionTracking:
                    Debug.Log("Supported, apk installed. SessionTracking...");
                    instructionText.SetText("AR Supported, apk installed.");
                    NextStep(true);
                    break;
                case ARSessionState.Unsupported:
                    Debug.Log("Unsupported, Device Not Capable");
                    instructionText.SetText("AR Unsupported, Device Not Capable");
                    NextStep(false);
                    break;
                case ARSessionState.None:
                    Debug.Log("Unsupported, Device Not Capable");
                    instructionText.SetText("AR Unsupported, Device Not Capable");
                    NextStep(false);
                    break;
                default:
                    Debug.Log("Unsupported, Device Not Capable");
                    instructionText.SetText("AR Unsupported, Device Not Capable");
                    NextStep(false);
                    break;
            }
        }
        else
        {
            Debug.Log("PC: AR not supported, Device Not Capable");
            NextStep(false);
        }
    }

    private void ARSessionStateChanged(ARSessionStateChangedEventArgs obj)
    {
        Debug.Log("Inside ARSessionStateChanged delegate...");
        switch (ARSession.state)
        {
            case ARSessionState.CheckingAvailability:
                Debug.Log("Still Checking AR Availability...");
                instructionText.SetText("Still Checking AR Availability...");
                break;
            case ARSessionState.NeedsInstall:
                Debug.Log("Supported, not installed, requesting installation");
                instructionText.SetText("AR Supported, not installed, requesting installation");
                InstallMode();
                break;
            case ARSessionState.Installing:
                Debug.Log("Supported, apk installing");
                instructionText.SetText("AR Supported, apk installing");
                InstallMode();
                break;
            case ARSessionState.Ready:
                Debug.Log("Supported and installed");
                instructionText.SetText("AR Supported and installed");
                NextStep(true);
                break;
            case ARSessionState.SessionInitializing:
                Debug.Log("Supported, apk installed. SessionInitializing...");
                instructionText.SetText("AR Supported, apk installed.");
                NextStep(true);
                break;
            case ARSessionState.SessionTracking:
                Debug.Log("Supported, apk installed. SessionTracking...");
                instructionText.SetText("AR Supported, apk installed.");
                NextStep(true);
                break;
            case ARSessionState.Unsupported:
                Debug.Log("Unsupported, Device Not Capable");
                instructionText.SetText("AR Unsupported, Device Not Capable");
                NextStep(false);
                break;
            case ARSessionState.None:
                Debug.Log("Unsupported, Device Not Capable");
                instructionText.SetText("AR Unsupported, Device Not Capable");
                NextStep(false);
                break;
            default:
                Debug.Log("Unsupported, Device Not Capable");
                instructionText.SetText("AR Unsupported, Device Not Capable");
                NextStep(false);
                break;
        }
    }

    private IEnumerator InstallARCoreApp() //Calls the method to install AR Core via the Play Store
    {
        yield return ARSession.Install();
    }


    private void NextStep(bool IsARSupported) //Enables mode choice according to device capabilities
    {
        if (!isNextStepDone)
        {
            isNextStepDone = true;
            ARSession.stateChanged -= ARSessionStateChanged;
            if (ARSessionObj)
            {
                Destroy(ARSessionObj);
            }

            if (IsARSupported)
            {
                roomScaleButton.GetComponent<Button>().interactable = true;
                messageText.SetText("Choose a mode \nto begin.");
            }
            else
            {
                roomScaleButton.GetComponent<Button>().interactable = false;

                if (!(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer))
                {             
                    messageText.SetText("AR unsupported, you may use this phone for Seated mode");
                }
                else
                {
                    messageText.SetText("AR unsupported, please use PC mode");
                }
                    
            }
        }
    }

    void InstallMode()
    {
        //disable ar phone button, add instruction, activate install button
        arCoreInstallButton.SetActive(true);
        roomScaleButton.GetComponent<Button>().enabled = false;
        roomScaleButton.GetComponent<Button>().interactable = false;
        messageText.SetText("Install AR Core \nfor Room Scale mode.");
    }

    public void InstallARCore()
    {
        StartCoroutine(InstallARCoreApp());
    }

}
