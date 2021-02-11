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
    public GameObject ARSessionPrefab;
    private GameObject ARSessionObj;

    private bool isARSupported;

    private bool triedInstall = false;

    public GameObject roomScaleButton;

    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI messageText;
    public GameObject arCoreInstallButton;

    bool isGyro = false;

    bool isNextStepDone = false;

    bool arCheck = false;

    //PC Mode Button
    public GameObject pcModeButton;

    private void Start()
    {

#if UNITY_EDITOR
        pcModeButton.SetActive(true);
#else
        pcModeButton.SetActive(false);
#endif

        messageText.SetText("Please wait...");

        roomScaleButton.GetComponent<Button>().interactable = false;
        arCoreInstallButton.SetActive(false);


        isNextStepDone = false;
        StartCoroutine(CheckAvailability());
        arCheck = true;


        ARSessionPrefab.GetComponent<ARSession>().attemptUpdate = false;

    }

    private IEnumerator CheckAvailability()
    {
        instructionText.SetText("Checking Availability...");

        if (!ARSessionObj)
        {
            ARSessionObj = Instantiate(ARSessionPrefab);
            //ARSessionObj.GetComponent<ARSession>().attemptUpdate = false;

            yield return new WaitForSecondsRealtime(0.5f);
        }
        if (!Application.isEditor)
        {
            Debug.Log("ARSession.state: " + ARSession.state);
            switch (ARSession.state)
            {
                case ARSessionState.CheckingAvailability:
                    Debug.Log("Still Checking Availability...");
                    instructionText.SetText("Still Checking Availability...");
                    ARSession.stateChanged += ARSessionStateChanged;
                    break;
                case ARSessionState.NeedsInstall:
                    Debug.Log("Supported, not installed, requesting installation");
                    instructionText.SetText("Supported, not installed, requesting installation");
                    InstallMode();
                    break;
                case ARSessionState.Installing:
                    Debug.Log("Supported, apk installing");
                    instructionText.SetText("Supported, apk installing");
                    InstallMode();
                    break;
                case ARSessionState.Ready:
                    Debug.Log("Supported and installed");
                    instructionText.SetText("Supported and installed");
                    NextStep(true);
                    break;
                case ARSessionState.SessionInitializing:
                    Debug.Log("Supported, apk installed. SessionInitializing...");
                    instructionText.SetText("Supported, apk installed.");
                    NextStep(true);
                    break;
                case ARSessionState.SessionTracking:
                    Debug.Log("Supported, apk installed. SessionTracking...");
                    instructionText.SetText("Supported, apk installed.");
                    NextStep(true);
                    break;
                case ARSessionState.Unsupported:
                    Debug.Log("Unsupported, Device Not Capable");
                    instructionText.SetText("Unsupported, Device Not Capable");
                    NextStep(false);
                    break;
                case ARSessionState.None:
                    Debug.Log("Unsupported, Device Not Capable");
                    instructionText.SetText("Unsupported, Device Not Capable");
                    NextStep(false);
                    break;
                default:
                    Debug.Log("Unsupported, Device Not Capable");
                    instructionText.SetText("Unsupported, Device Not Capable");
                    NextStep(false);
                    break;
            }
        }
        else
        {
            Debug.Log("Unity editor: AR not supported, Device Not Capable");
            NextStep(false);
        }
    }

    private void ARSessionStateChanged(ARSessionStateChangedEventArgs obj)
    {
        Debug.Log("Inside ARSessionStateChanged delegate...");
        switch (ARSession.state)
        {
            case ARSessionState.CheckingAvailability:
                Debug.Log("Still Checking Availability...");
                instructionText.SetText("Still Checking Availability...");
                break;
            case ARSessionState.NeedsInstall:
                Debug.Log("Supported, not installed, requesting installation");
                instructionText.SetText("Supported, not installed, requesting installation");
                InstallMode();
                break;
            case ARSessionState.Installing:
                Debug.Log("Supported, apk installing");
                instructionText.SetText("Supported, apk installing");
                InstallMode();
                break;
            case ARSessionState.Ready:
                Debug.Log("Supported and installed");
                instructionText.SetText("Supported and installed");
                NextStep(true);
                break;
            case ARSessionState.SessionInitializing:
                Debug.Log("Supported, apk installed. SessionInitializing...");
                instructionText.SetText("Supported, apk installed.");
                NextStep(true);
                break;
            case ARSessionState.SessionTracking:
                Debug.Log("Supported, apk installed. SessionTracking...");
                instructionText.SetText("Supported, apk installed.");
                NextStep(true);
                break;
            case ARSessionState.Unsupported:
                Debug.Log("Unsupported, Device Not Capable");
                instructionText.SetText("Unsupported, Device Not Capable");
                NextStep(false);
                break;
            case ARSessionState.None:
                Debug.Log("Unsupported, Device Not Capable");
                instructionText.SetText("Unsupported, Device Not Capable");
                NextStep(false);
                break;
            default:
                Debug.Log("Unsupported, Device Not Capable");
                instructionText.SetText("Unsupported, Device Not Capable");
                NextStep(false);
                break;
        }
    }


    private void Update()
    {
        if (arCheck == false)
        {
            //StartCoroutine(CheckAvailability());
            //arCheck = true;
        }
    }

    private IEnumerator InstallARCoreApp()
    {
        yield return ARSession.Install();
        //NextStep(true);
    }



    private void NextStep(bool IsARSupported)
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

                roomScaleButton.GetComponent<Button>().enabled = true;
                roomScaleButton.GetComponent<Button>().interactable = true;
                messageText.SetText("Choose a mode \nto begin.");

            }
            else
            {
                //Deactivate button and grey it out
                //Instruction text, check if Gyro

                roomScaleButton.GetComponent<Image>().color = Color.gray;
                roomScaleButton.GetComponent<Button>().enabled = false;
                roomScaleButton.GetComponent<Button>().interactable = false;
                messageText.SetText("You can only use \nthis phone \nfor seated mode.");

            }



        }



    }

    void InstallMode()
    {
        //grey out ar phone button, add instruction, activate install button

        arCoreInstallButton.SetActive(true);
        roomScaleButton.GetComponent<Image>().color = Color.gray;
        roomScaleButton.GetComponent<Button>().enabled = false;
        roomScaleButton.GetComponent<Button>().interactable = false;
        messageText.SetText("Install AR Core \nfor Room Scale mode.");
    }

    public void InstallARCore()
    {
        StartCoroutine(InstallARCoreApp());
    }

}
