using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockSpriteHandler : MonoBehaviour
{

    //Convoluted Lock Sprite Handler

    [Header("Lock & Unlock Sprites")]
    public Sprite lockSprite;
    public Sprite unlockSprite;

    
    Image myImage;
    Button myButton;
    bool objectLock = true;


    void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        objectLock = true; //Object locked by default

        myButton.onClick.AddListener(ToggleLock);

        if(lockSprite!=null && unlockSprite!=null) //If sprites are available, set sprite, else set colour
        {
            myImage.sprite = objectLock ? lockSprite : unlockSprite;       
        }
        else
        {
            myImage.color = objectLock ? Color.white : Color.green;
        }
    }

    public void ToggleLock()
    {
        if(RoomManager.instance!=null) // CHECK! Old implementation, only changes colour
        {
            RoomManager.instance.ObjectLockToggle();
            objectLock = RoomManager.instance.IsObjectLocked();
            myImage.color = objectLock ? Color.white : Color.green;
        }
        else //If sprites are available, change sprites
        {
            if (objectLock == false)
            {
                objectLock = true;
            }
            else
            {
                objectLock = false;
            }

            if (lockSprite != null && unlockSprite != null)
            {
                myImage.sprite = objectLock ? lockSprite : unlockSprite;
            }
            else //Fallback in case I forget things like an idiot
            {
                myImage.color = objectLock ? Color.white : Color.green;
            }
        }
    }
}
