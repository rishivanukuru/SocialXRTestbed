using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EndUIAnimation : MonoBehaviour
{

    public static event Action onPlacedObject;

    public ARPlacementInteractableUIBlocked aRPlacementInteractableUIBlocked;

    public void StopAnimation()
    {
        onPlacedObject();
        aRPlacementInteractableUIBlocked.isObjectPlaced = true;
    }
}