using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockSpriteHandler : MonoBehaviour
{

    Image myImage;
    bool objectLock = true;
    Button myButton;

    void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ToggleLock);
        objectLock = true;
        myImage.color = objectLock ? Color.white : Color.green;
    }

    void Update()
    {
        
    }

    public void ToggleLock()
    {
        if(RoomManager.instance!=null)
        {
            RoomManager.instance.ObjectLockToggle();
            objectLock = RoomManager.instance.IsObjectLocked();
            myImage.color = objectLock ? Color.white : Color.green;
        }
       
    }
}
