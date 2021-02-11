using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARTrackedImageManager))]
public class CustomImageTrackerOne : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;
    private Dictionary<string, GameObject> spawnedPrefab = new Dictionary<string, GameObject>();

    private ARTrackedImageManager trackedImageManager;

    public GameObject GlobalImageObjectPrefab;

    public GameObject InSceneObject;

    GameObject globalImageObject;

    public bool activateGlobalImage = true;
    public bool activateSceneObject = false;

    string nameOfImage;

    public bool isImageTracked = false;

    public bool firstImageTracked = false;


    public bool displayRotation = false;
    public GameObject displayRotationObject;


    // Start is called before the first frame update
    void Awake()
    {
        globalImageObject = Instantiate(GlobalImageObjectPrefab);
        globalImageObject.SetActive(false);
        displayRotation = false;
        displayRotationObject.SetActive(false);
        //InSceneObject.transform.position = farAway;

        InSceneObject.SetActive(false);

        firstImageTracked = false;

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            spawnedPrefab.Add(newPrefab.name, newPrefab);
        }

        isImageTracked = false;
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefab[trackedImage.name].SetActive(false);
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(!isImageTracked)
            {
                isImageTracked = true;
            }

            if (firstImageTracked == false)
            {
                firstImageTracked = true;
               
            }

            if (displayRotation)
            {
                displayRotationObject.SetActive(false);
            }


            if (activateSceneObject)
            {
                InSceneObject.SetActive(true);
                InSceneObject.transform.position = trackedImage.transform.position;
                InSceneObject.transform.rotation = trackedImage.transform.rotation;
            }
            else
            if (activateGlobalImage)
            {
                globalImageObject.SetActive(true);
                globalImageObject.transform.position = trackedImage.transform.position;
                globalImageObject.transform.rotation = trackedImage.transform.rotation;
            }
            else
            {
                nameOfImage = trackedImage.referenceImage.name;

                spawnedPrefab[nameOfImage].SetActive(true);

                spawnedPrefab[nameOfImage].transform.position = trackedImage.transform.position;

                spawnedPrefab[nameOfImage].transform.rotation = trackedImage.transform.rotation;

            }

        }
        else
        {
            if (isImageTracked)
            {
                isImageTracked = false;
            }

            if (displayRotation)
            {
                displayRotationObject.SetActive(true);
            }

            if (activateSceneObject)
            {
                InSceneObject.SetActive(false);
                //InSceneObject.transform.position = farAway;
            }
            else
            if (activateGlobalImage)
            {
                globalImageObject.SetActive(false);
            }
            else
            {
                nameOfImage = trackedImage.referenceImage.name;
                spawnedPrefab[nameOfImage].SetActive(false);
            }

        }
    }

}
