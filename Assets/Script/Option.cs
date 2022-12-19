using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown DResolution;

    public static float musicVolume {get;  private set; }
    public static float soundEffectVolume { get; private set; }
    
    private void Awake()
    {
        DResolution.value = 2;
    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }
    
    public void OnSoundeEffectsSliderValueChange(float value)
    {
        soundEffectVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void SetResolution()
    {
        switch (DResolution.value)
        {
            case 0:
                Screen.SetResolution(800 , 600 , true);
                break;
            case 1:
                Screen.SetResolution(1280 , 1080 , true);
                break; 
            case 2:
                Screen.SetResolution(1920 , 1080 , true);
                break;
        }
    }

}
