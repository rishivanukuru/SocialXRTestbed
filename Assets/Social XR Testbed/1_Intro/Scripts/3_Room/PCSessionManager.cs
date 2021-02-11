using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSessionManager : MonoBehaviour
{
    [Header("PC Prefabs")]
    public GameObject referenceObjectPC;

    [Header("PC Reference Location")]
    public Transform referenceObjectTransformPC;

    private bool isPlaced = false;

    public GameObject spawnedObject { get; private set; }


    void Start()
    {
        isPlaced = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)&&!isPlaced)
        {
            isPlaced = true;
            InitPlayerPC();
        }
    }

    void InitPlayerPC()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(referenceObjectPC, referenceObjectTransformPC.position, referenceObjectTransformPC.rotation);

            spawnedObject.transform.Rotate(Vector3.up, -45f * BasicARSessionManager.instance.OtherPlayerList.Count, Space.Self);

            BasicARSessionManager.instance.referenceObject = spawnedObject;

            BasicARSessionManager.instance.SpawnPlayer();
        }
    }
}
