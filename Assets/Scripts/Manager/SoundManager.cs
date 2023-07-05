using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public float backGroundVolume;
    public float sfxVolume;

    public Transform backGroundSoundObject;
    public Transform sfxSoundObject;


    public void PlaySound(string name)
    {

    }

    /// <summary>
    /// 배경음악소리 크기를 바꾸는 함수
    /// </summary>
    /// <param name="value">바꿀 크기</param>
    public void SetBackGroundVolume(float value)
    {
        backGroundVolume = value;
        AudioSource[] playingBackGroundSoundList = backGroundSoundObject.GetComponentsInChildren<AudioSource>();

    }



}
