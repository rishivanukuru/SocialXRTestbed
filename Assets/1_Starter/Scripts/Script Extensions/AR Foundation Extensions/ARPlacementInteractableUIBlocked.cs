using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARPlacementInteractableUIBlocked : ARPlacementInteractable
{

    public bool isObjectPlaced = false;

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {

        if (gesture.StartPosition.IsPointOverUIObject())
        {
            return false;
        }

        if (isObjectPlaced)
        {
            if (gesture.StartPosition.IsPointOverUIObject())
            {
                return false;
            }

            // Allow for test planes
            if (gesture.TargetObject == null || gesture.TargetObject.layer == 9)
                return true;

            return false;
        }

        return true;

    }

}
