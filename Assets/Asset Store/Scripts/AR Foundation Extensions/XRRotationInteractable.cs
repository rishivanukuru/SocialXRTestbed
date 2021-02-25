using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;


public class XRRotationInteractable : ARRotationInteractable
{
    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.StartPosition.IsPointOverUIObject())
        {
            return false;
        }
        else
        if (RoomManager.instance != null)
        {
            if (RoomManager.instance.IsObjectLocked())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true; 
        }


       
    }
}
