using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsMaster : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenTog;
    [SerializeField] private TMP_Dropdown resDropDown;
    
    void Start()
    {
        Screen.SetResolution(Screen.width,Screen.height, fullscreenTog.isOn);

        //populate dropdown with available resolutions
        List<string> resolutions = new List<string>();
        int currentResIndex=0;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i].width + " X " + Screen.resolutions[i].height);

            if (Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height)
                currentResIndex = i;
        }
        
        resDropDown.ClearOptions();
        resDropDown.AddOptions(resolutions);
        
        //select current resolution in dropdown
        string currentRes = Screen.width + " X " + Screen.height;
        resDropDown.options[currentResIndex].text = currentRes;
        resDropDown.transform.GetComponentInChildren<TextMeshProUGUI>().text = currentRes;

    }

    public void ApplyGraphics()
    {
        FullScreenMode mode;
        string [] res = resDropDown.options[resDropDown.value].text.Split(" X ");
        
        Screen.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), fullscreenTog.isOn);
    }
}
