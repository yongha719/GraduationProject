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

    private List<AudioClip> dialogueRecordingList = new List<AudioClip>();

    private Dictionary<string, AudioClip> backGroundSoundClipDic = new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> sfxSoundClipDic = new Dictionary<string , AudioClip>();

    private Dictionary<string, AudioClip> dialogueRecordingDic = new Dictionary<string, AudioClip>();


    public void PlayBackGroundSound(string name)
    {
        GameObject go = new GameObject(backGroundSoundClipDic[name] + "Sound");
        go.transform.parent = backGroundSoundObject;
        go.AddComponent<AudioSource>().clip = backGroundSoundClipDic[name];
        go.GetComponent<AudioSource>().Play();
    }

    public void PlaySFXSound(string name)
    {
        GameObject go = new GameObject(sfxSoundClipDic[name] + "Sound");
        go.transform.parent = sfxSoundObject;
        go.AddComponent<AudioSource>().clip = sfxSoundClipDic[name];
        go.GetComponent<AudioSource>().Play();

        Destroy(go, sfxSoundClipDic[name].length);
    }

    public void PlayDialogue(string name)
    {
        GameObject go = new GameObject(dialogueRecordingDic[name] + "Sound");
        go.transform.parent = sfxSoundObject;
        go.AddComponent<AudioSource>().clip = dialogueRecordingDic[name];
        go.GetComponent<AudioSource>().Play();

        Destroy(go, dialogueRecordingDic[name].length);
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

        for (int i = 0; i < backGroundSoundClipList.Count; i++)
        {
            backGroundSoundClipDic.Add(backGroundSoundClipList[i].name, backGroundSoundClipList[i]);
        }

        Object[] sfxObjs = Resources.LoadAll("/Sound/SFX");
        foreach(Object obj in sfxObjs)
        {
            sfxSoundClipList.Add((AudioClip)obj);
        }

        for (int i = 0; i < sfxSoundClipList.Count; i++)
        {
            sfxSoundClipDic.Add(sfxSoundClipList[i].name, sfxSoundClipList[i]);
        }

        Object[] dialongObjs = Resources.LoadAll("/Sound/DialogueRecording");
        foreach(Object obj in dialongObjs)
        {
            dialogueRecordingList.Add((AudioClip)obj);
        }

        for (int i = 0; i < dialogueRecordingList.Count; i++)
        {
            dialogueRecordingDic.Add(dialogueRecordingList[i].name, dialogueRecordingList[i]);
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
            playingBackGroundSoundList[i].volume = value;
        }
    }
}
