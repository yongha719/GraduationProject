using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public float backGroundVolume;
    public float sfxVolume;

    public Transform backGroundSoundObject;
    public Transform sfxSoundObject;

    private List<AudioClip> backGroundSoundClipList = new List<AudioClip>();

    private List<AudioClip> sfxSoundClipList = new List<AudioClip>();

    private Dictionary<string, AudioClip> backGroundSoundClipDic = new Dictionary<string, AudioClip>();


    public void PlayBackGroundSound(string name)
    {

    }

    public void PlaySFXSound(string name)
    {

    }

    private void Start()
    {
        LoadSound();
    }

    private void LoadSound()
    {
        Object[] backObjs = Resources.LoadAll("/Sound/BackGround");
        foreach(Object obj in backObjs)
        {
            backGroundSoundClipList.Add((AudioClip)obj);
        }

        Object[] sfxObjs = Resources.LoadAll("/Sound/SFX");
        foreach(Object obj in sfxObjs)
        {
            sfxSoundClipList.Add((AudioClip)obj);
        }
    }

    /// <summary>
    /// 배경음악소리 크기를 바꾸는 함수
    /// </summary>
    /// <param name="value">바꿀 크기</param>
    public void SetBackGroundVolume(float value)
    {
        backGroundVolume = value;
        AudioSource[] playingBackGroundSoundList = backGroundSoundObject.GetComponentsInChildren<AudioSource>();

        if(playingBackGroundSoundList.Length == 0)
        {
            return;
        }


        for (int i = 0; i < playingBackGroundSoundList.Length; i++)
        {

        }
    }




}
