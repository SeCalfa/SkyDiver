using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Saving : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Record = null;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Record"))
        {
            PlayerPrefs.SetInt("Record", 0);
        }
        else
        {
            Record.text = "You record: " + PlayerPrefs.GetInt("Record");
        }
    }
}
