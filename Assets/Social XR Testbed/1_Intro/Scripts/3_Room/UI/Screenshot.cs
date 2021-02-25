using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    public string fileName = "ScholAR_Screenshot_";
    public GameObject mainUI;
    public GameObject watermarkUI;
    AudioSource screenshotSound;
    int currentScreenshotNumber;
  
    void Start()
    {
        watermarkUI.SetActive(false);
        screenshotSound = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("screenshot_number"))
        {
            currentScreenshotNumber = PlayerPrefs.GetInt("screenshot_number");
        }
        else
        {
            PlayerPrefs.SetInt("screenshot_number", 0);
            currentScreenshotNumber = 0;
        }
    }


    public void TakeScreenshot()
    {
        mainUI.SetActive(false);
        if(RoomManager.instance!=null)
        {
            if(RoomManager.instance.isPlaced)
            {
                RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().handHolder.SetActive(false);
            }
        }
        watermarkUI.SetActive(true);
        StartCoroutine(captureScreenshot());
    }

    IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        string screenshot_name = fileName + currentScreenshotNumber.ToString();

        string filePath = Application.persistentDataPath + "\\"
            + screenshot_name.ToString() + "_" + Screen.width + "X" + Screen.height + "" + ".png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to file
        if(Application.isEditor)
        {
            System.IO.File.WriteAllBytes(filePath, imageBytes);
            Debug.Log("PC - Attempted Screenshot with Local File Path: " + filePath);
        }
        else
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(screenImage, "ScholAR Experiments", screenshot_name + ".jpg", (success, path) => Debug.Log("Media save result: " + success + " " + path));
            Debug.Log("Android - Attempted Screenshot with permission: " + permission.ToString());
        }

        currentScreenshotNumber++;
        PlayerPrefs.SetInt("screenshot_number", currentScreenshotNumber);

        watermarkUI.SetActive(false);
        if (RoomManager.instance != null)
        {
            if (RoomManager.instance.isPlaced)
            {
                RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().handHolder.SetActive(true);
            }
        }
        mainUI.SetActive(true);
        screenshotSound.Play();

    }

}
