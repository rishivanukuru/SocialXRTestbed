using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HostPasswordValidate : MonoBehaviour
{

    [SerializeField] private string hostPassword = "123";
    [SerializeField] private TMP_InputField password;

    [SerializeField] private GameObject currentPanel;
    [SerializeField] private GameObject nextPanel;


    public void ValidatePassword()
    {

        if (password.text == hostPassword)
        {
            currentPanel.SetActive(false);
            nextPanel.SetActive(true);
        }

    }



}
