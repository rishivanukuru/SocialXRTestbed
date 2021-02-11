using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPanelController : MonoBehaviour
{
    public AvatarChangeManager avatarChangeManager;
    public GameObject drawPanel;

    void Start()
    {
        drawPanel.SetActive(false);
    }

    void Update()
    {
        if(RoomManager.instance.isPlaced)
        {
            
            if (RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentIndex == 0)
            {

                if(drawPanel.activeSelf)
                {
                    drawPanel.SetActive(false);
                }
            }
            else
            {
                if (!drawPanel.activeSelf)
                {
                    drawPanel.SetActive(true);
                }
            }
        }
    }
}
