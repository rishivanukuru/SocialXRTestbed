using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ARWordGenerator : MonoBehaviour
{


    private TMP_Text headerText;
    
    void Start()
    {
        string [] arNames = {
            "ARtichoke",
            "ARmadillo",
            "ARtemis",
            "Random AR Maker",
            "ARcadia",
            "ARe you there"
        };
        
        headerText = this.GetComponent<TMP_Text>();
        headerText.text = "Welcome to " + arNames[Random.Range(0,arNames.Length)] + ".";

    }

    void Update()
    {
        
    }
}
