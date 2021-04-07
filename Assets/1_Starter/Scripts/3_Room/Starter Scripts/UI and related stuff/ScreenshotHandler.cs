using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    //Handles native screenshot taking for AR and non-AR mode
    //See Screenshot script for more details

    public Screenshot arScreenShot;
    public Screenshot normalScreenShot;

    public void TakeScreenshot() //Checks mode, takes appropriate screenshot
    {
        if(XRModeManager.xrMode == 1 || XRModeHandler.xrMode == 1)
        {
            arScreenShot.TakeScreenshot();
        }
        else
        if (XRModeManager.xrMode == 2 || XRModeHandler.xrMode == 2)
        {
            normalScreenShot.TakeScreenshot();
        }
    }
}
