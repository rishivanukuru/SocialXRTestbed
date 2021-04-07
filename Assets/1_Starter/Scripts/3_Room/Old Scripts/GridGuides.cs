using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGuides : MonoBehaviour
{

    [Header("Grids")]
    public GameObject gridXY;
    public GameObject gridXZ;
    public GameObject gridYZ;

    [Header("Buttons")]
    public Button guideXY;
    public Button guideXZ;
    public Button guideYZ;



    void Start()
    {
        guideXY.onClick.AddListener(ToggleXY);
        guideXZ.onClick.AddListener(ToggleXZ);
        guideYZ.onClick.AddListener(ToggleYZ);
    }


    public void ToggleXY()
    {
        if(gridXY.activeSelf)
        {
            guideXY.gameObject.GetComponent<Image>().color = Color.white;
            gridXY.SetActive(false);

        }
        else
        {
            guideXY.gameObject.GetComponent<Image>().color = Color.green;

            gridXY.transform.position = RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().myDraw.drawPoint.transform.position;
            gridXY.transform.forward = Vector3.ProjectOnPlane(RoomManager.instance.PlayerRef.transform.forward, Vector3.up).normalized;
            gridXY.SetActive(true);

        }
    }

    public void ToggleXZ()
    {
        if (gridXZ.activeSelf)
        {
            guideXZ.gameObject.GetComponent<Image>().color = Color.white;

            gridXZ.SetActive(false);
        }
        else
        {
            guideXZ.gameObject.GetComponent<Image>().color = Color.green;

            gridXZ.transform.position = RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().myDraw.drawPoint.transform.position;
            gridXZ.transform.forward = Vector3.ProjectOnPlane(RoomManager.instance.PlayerRef.transform.forward, Vector3.up).normalized;
            gridXZ.SetActive(true);
        }
    }

    public void ToggleYZ()
    {
        if (gridYZ.activeSelf)
        {
            guideYZ.gameObject.GetComponent<Image>().color = Color.white;

            gridYZ.SetActive(false);
        }
        else
        {
            guideYZ.gameObject.GetComponent<Image>().color = Color.green;
            gridYZ.transform.position = RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().myDraw.drawPoint.transform.position;
            gridYZ.transform.forward = Vector3.ProjectOnPlane(RoomManager.instance.PlayerRef.transform.forward, Vector3.up).normalized;
            gridYZ.SetActive(true);
        }
    }

}
