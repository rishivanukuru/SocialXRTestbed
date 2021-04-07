using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    //Script to take a screenshot and store it in the Android Gallery/PC folder

    [Header("Screenshot File Name Starter")]
    public string fileName = "ScholAR_Screenshot_";

    [Header("UI Panels")]
    public GameObject mainUI; //UI to be hidden
    public GameObject watermarkUI; //UI to be shown (currently the IMXD logo)

    AudioSource screenshotSound; //Gameobject must have Audiosource attached
    int currentScreenshotNumber; //Running count of number of screenshots to avoid duplicate filenames
  
    void Start()
    {
        watermarkUI.SetActive(false);
        screenshotSound = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("screenshot_number"))//Checks for screenshot counter, assigns count if it exists, creates new one if not
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
        mainUI.SetActive(false); //Deactivates UI

        if(RoomManager.instance!=null) //CHECK! Removes hand from view in old implementation
        {
            if(RoomManager.instance.isPlaced)
            {
                RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().handHolder.SetActive(false);
            }
        }
        watermarkUI.SetActive(true); //Activates watermark
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

        //Increments screenshot counter
        currentScreenshotNumber++;
        PlayerPrefs.SetInt("screenshot_number", currentScreenshotNumber);

        //Removes watermark
        watermarkUI.SetActive(false);

        //CHECK! Brings back hand in the old implementation
        if (RoomManager.instance != null)
        {
            if (RoomManager.instance.isPlaced)
            {
                RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().handHolder.SetActive(true);
            }
        }

        //Brings back UI
        mainUI.SetActive(true);

        //Plays photo click sound
        screenshotSound.Play();

    }

}
