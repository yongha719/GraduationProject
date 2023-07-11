using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum CommanderSoundType
{
    MaleCommander,
    FemaleCommander
}
public class SoundManager : Singleton<SoundManager>
{
    public float backGroundVolume;
    public float sfxVolume;

    public Transform backGroundSoundObject;
    public Transform sfxSoundObject;

    private List<AudioClip> backGroundSoundClipList = new List<AudioClip>();

    private List<AudioClip> sfxSoundClipList = new List<AudioClip>();

    private List<AudioClip> manDialogueRecordingList = new List<AudioClip>();

    private List<AudioClip> womanDialogueRecordingList = new List<AudioClip>();

    private Dictionary<string, AudioClip> backGroundSoundClipDic = new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> sfxSoundClipDic = new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> manDialogueRecordingDic = new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> womanDialogueRecordingDic = new Dictionary<string, AudioClip>();

    private List<GameObject> playingBackGroundSound = new List<GameObject>();

    public void PlayBackGroundSound(string name)
    {
        return;

        GameObject go = new GameObject(backGroundSoundClipDic[name] + "Sound");
        go.transform.parent = backGroundSoundObject;
        go.AddComponent<AudioSource>().PlayOneShot(backGroundSoundClipDic[name]);
        go.GetComponent<AudioSource>().loop = true;
        playingBackGroundSound.Add(go);
    }

    public void AllStopBackGroundSound()
    {
        for (int i = 0; i < playingBackGroundSound.Count; i++)
        {
            Destroy(playingBackGroundSound[i]);
        }
    }

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaySFXSound("Baekyura");
        }
    }


    public void PlaySFXSound(string name)
    {
        return;

        GameObject go = new GameObject(sfxSoundClipDic[name] + "Sound");
        go.transform.parent = sfxSoundObject;

        go.AddComponent<AudioSource>().PlayOneShot(sfxSoundClipDic[name]);

        Destroy(go, sfxSoundClipDic[name].length);
    }

    //ItsMyTurn
    public void PlayDialogue(bool isMan)
    {
        return;

        var soundName = Random.Range(0, 2) == 0 ? "ItsMyTurn" : "MyTurn";
        PlayDialogue(soundName, isMan);
    }

    public void PlayDialogue(string name, bool isMan)
    {
        return;

        if (isMan)
        {
            GameObject go = new GameObject(manDialogueRecordingDic[name] + "Sound");
            go.transform.parent = sfxSoundObject;
            go.AddComponent<AudioSource>().PlayOneShot(manDialogueRecordingDic[name]);

            Destroy(go, manDialogueRecordingDic[name].length);
        }
        else
        {
            GameObject go = new GameObject(womanDialogueRecordingDic[name] + "Sound");
            go.transform.parent = sfxSoundObject;
            go.AddComponent<AudioSource>().PlayOneShot(womanDialogueRecordingDic[name]);

            Destroy(go, womanDialogueRecordingDic[name].length);
        }

    }

    private void Start()
    {
        LoadSound();
    }

    private void LoadSound()
    {
        Object[] backObjs = Resources.LoadAll("Sound/BackGround");
        foreach (Object obj in backObjs)
        {
            backGroundSoundClipList.Add((AudioClip)obj);
        }

        for (int i = 0; i < backGroundSoundClipList.Count; i++)
        {
            backGroundSoundClipDic.Add(backGroundSoundClipList[i].name, backGroundSoundClipList[i]);
        }

        Object[] sfxObjs = Resources.LoadAll("Sound/SFX");
        foreach (Object obj in sfxObjs)
        {
            sfxSoundClipList.Add((AudioClip)obj);
        }

        for (int i = 0; i < sfxSoundClipList.Count; i++)
        {
            sfxSoundClipDic.Add(sfxSoundClipList[i].name, sfxSoundClipList[i]);
        }

        Object[] manDialongObjs = Resources.LoadAll("Sound/DialogueRecording/Man");
        foreach (Object obj in manDialongObjs)
        {
            manDialogueRecordingList.Add((AudioClip)obj);
        }

        for (int i = 0; i < manDialogueRecordingList.Count; i++)
        {
            manDialogueRecordingDic.Add(manDialogueRecordingList[i].name, manDialogueRecordingList[i]);
        }

        Object[] womanDialongObjs = Resources.LoadAll("Sound/DialogueRecording/Woman");
        foreach (Object obj in womanDialongObjs)
        {
            womanDialogueRecordingList.Add((AudioClip)obj);
        }

        for (int i = 0; i < manDialogueRecordingList.Count; i++)
        {
            womanDialogueRecordingDic.Add(womanDialogueRecordingList[i].name, womanDialogueRecordingList[i]);
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

        if (playingBackGroundSoundList.Length == 0)
        {
            return;
        }


        for (int i = 0; i < playingBackGroundSoundList.Length; i++)
        {
            playingBackGroundSoundList[i].volume = value;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayDialogue("Iknow", false);
        }
    }
}
