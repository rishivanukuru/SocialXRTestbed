using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPanelController : MonoBehaviour
{
    public AvatarChangeManager avatarChangeManager;
    public GameObject drawPanel;
    public GameObject colorPanel;
    public Button colorPickerToggle;

    void Start()
    {
        drawPanel.SetActive(false);
        colorPanel.SetActive(false);
        colorPickerToggle.onClick.AddListener(ColorPickerToggle);
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

                if(colorPanel.activeSelf)
                {
                    colorPanel.SetActive(false);
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

    void ColorPickerToggle()
    {
        if(colorPanel.activeSelf)
        {
            colorPanel.SetActive(false);
        }
        else
        {
            colorPanel.SetActive(true);
        }
    }
}
