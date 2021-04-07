using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARTrackedImageManager))]
public class CustomImageTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;
    private Dictionary<string, GameObject> spawnedPrefab = new Dictionary<string, GameObject>();

    private ARTrackedImageManager trackedImageManager;

    public GameObject GlobalImageObjectPrefab;

    public GameObject InSceneObject;
    public Vector3 farAway;

    GameObject globalImageObject;

    public bool activateGlobalImage = true;
    public bool activateSceneObject = false;

    string nameOfImage;

    public bool isImageTracked = false;


    // Start is called before the first frame update
    void Awake()
    {
        globalImageObject = Instantiate(GlobalImageObjectPrefab);
        globalImageObject.SetActive(false);

        farAway.Set(100, 100, 100);
        //InSceneObject.transform.position = farAway;

        InSceneObject.SetActive(false);


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

            isImageTracked = true;


            if(activateSceneObject)
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
            isImageTracked = false;

            if(activateSceneObject)
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
