using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TableObjectManager : MonoBehaviour
{

    public GameObject tableObjectHolder;
    [HideInInspector]
    public GameObject currentObject;
    [HideInInspector]
    public int tableObjects;
    [HideInInspector]
    public int currentIndex;

    public VideoPlayer syncVideoPlayer;

    // Start is called before the first frame update
    void Start()
    {

  

        tableObjects = tableObjectHolder.transform.childCount;
        foreach(Transform child in tableObjectHolder.transform)
        {
            child.gameObject.SetActive(false);
        }

        currentIndex = 0;

        currentObject = tableObjectHolder.transform.GetChild(currentIndex).gameObject;
        tableObjectHolder.transform.GetChild(currentIndex).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextObject(int nextIndex)
    {
        tableObjectHolder.transform.GetChild(currentIndex).gameObject.SetActive(false);

        if (nextIndex == tableObjectHolder.transform.childCount)
        {
            currentIndex = 0;

        }
        else
        {
            currentIndex = nextIndex;
        }

        currentObject = tableObjectHolder.transform.GetChild(currentIndex).gameObject;
        tableObjectHolder.transform.GetChild(currentIndex).gameObject.SetActive(true);
    }

    public void StartVideo()
    {
        if (syncVideoPlayer != null)
        {
            if(syncVideoPlayer.isPlaying)
            {
                syncVideoPlayer.Pause();
            }
            else
            {
                syncVideoPlayer.Play();
            }

        }
    }

}
