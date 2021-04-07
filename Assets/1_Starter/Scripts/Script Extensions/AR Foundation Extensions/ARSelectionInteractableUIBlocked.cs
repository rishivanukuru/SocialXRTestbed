using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARSelectionInteractableUIBlocked : ARSelectionInteractable
{

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.StartPosition.IsPointOverUIObject()) //Don't start if over UI
        {
            return false;
        }
        else
        if(RoomManager.instance!=null) //Don't select if the object is locked
        {
            if(RoomManager.instance.IsObjectLocked())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        if (XRRoomManager.instance != null) //Don't select if the object is locked
        {
            if (XRRoomManager.instance.isObjectLocked())
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


        /*
        // Allow for test planes
        if (gesture.TargetObject == null || gesture.TargetObject.layer == 9)
            return false;
            */

        //return false;

        //return true;
    }


}
