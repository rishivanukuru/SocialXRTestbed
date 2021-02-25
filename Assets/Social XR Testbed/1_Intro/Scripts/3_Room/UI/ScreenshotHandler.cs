using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public Screenshot arScreenShot;
    public Screenshot normalScreenShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeScreenshot()
    {
        if(XRModeManager.xrMode == 1)
        {
            arScreenShot.TakeScreenshot();
        }
        else
        if (XRModeManager.xrMode == 2)
        {
            normalScreenShot.TakeScreenshot();
        }
    }
}
