using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider backGroundSoundBar;
    [SerializeField] private Slider sfxSoundBar;

    public float BackGroundSoundVolume
    {
        get
        {
            return backGroundSoundBar.value;
        }

        set
        {
            backGroundSoundBar.value = value;
            SoundManager.Instance.backGroundVolume = backGroundSoundBar.value;
        }
    }


    public float SFXSoundVolume
    {
        get
        {
            return sfxSoundBar.value;
        }
        set
        {
            sfxSoundBar.value = value;
            SoundManager.Instance.sfxVolume = sfxSoundBar.value;
        }
    }
    
    
    void Start()
    {
        
    }

}
