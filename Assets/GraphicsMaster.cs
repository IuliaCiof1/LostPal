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
    //[SerializeField] private TextMeshProUGUI text;
    void Start()
    {
        // Screen.fullScreen = true;
        // fullscreenTog.isOn = Screen.fullScreen;
        // text.text = Screen.fullScreen.ToString();
        Screen.SetResolution(Screen.width,Screen.height, fullscreenTog.isOn);
        //text.text = resDropDown.options[resDropDown.value].text;
        //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        //select in dropdown the current resolution
        //resDropDown.options[resDropDown.value].text = Screen.width + " X " + Screen.height;
        //Debug.Log(Screen.currentResolution.width + " x " + Screen.currentResolution.height);

        
        //populate dropdown with available resolutions
        List<string> resolutions = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i].width + " X " + Screen.resolutions[i].height);
            Debug.Log(Screen.resolutions[i].ToString());
        }
        
        resDropDown.ClearOptions();
        resDropDown.AddOptions(resolutions);
        resDropDown.options[resDropDown.value].text = Screen.width + " X " + Screen.height;
    }

    private int i = 0;
    public void ApplyGraphics()
    {
        FullScreenMode mode;
        string [] res = resDropDown.options[resDropDown.value].text.Split(" X ");
        //text.text = Int32.Parse(res[0]) + " " + res[1]+" "+Screen.width+" "+Screen.height;
        
        if (fullscreenTog.isOn)
        {
            mode = FullScreenMode.ExclusiveFullScreen;
            //text.text = "fullscreen";
        }
        else
        {
            mode = FullScreenMode.Windowed;
            //text.text = "windowed";
        }

        
        Debug.Log(i);
        i++;
        //Screen.fullScreen = fullscreenTog.isOn;

        // string [] res = resDropDown.options[resDropDown.value].text.Split(" x ");
        //Debug.Log(fullscreenTog.isOn+res[0]+" "+res[1]);
        //Debug.Log(Screen.currentResolution.width + " x " + Screen.currentResolution.height);
        //set resolution and fulscreen

        Screen.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), fullscreenTog.isOn);
        //Screen.SetResolution(1600, 900, mode);
        //Debug.Log(Screen.currentResolution.width + " x " + Screen.currentResolution.height);
        //text.text = Int32.Parse(res[0]) + " " + res[1]+""+Screen.currentResolution.ToString();
    }
}
