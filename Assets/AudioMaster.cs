using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMaster : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    public void Update()
    {
        //min, max values of sliders were changed to min, max of audio mixer
        //assign value from slider to the parameters defined in audio mixer
        mixer.SetFloat("MusicVol", musicSlider.value);
        mixer.SetFloat("SFXVol", SFXSlider.value);
    }
}
