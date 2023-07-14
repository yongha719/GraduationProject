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

        GameObject go = new GameObject($"{backGroundSoundClipDic[name]}Sound", typeof(AudioSource));
        go.transform.SetParent(backGroundSoundObject);

        var source = go.GetComponent<AudioSource>();

        source.PlayOneShot(backGroundSoundClipDic[name]);
        source.loop = true;

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
        LoadSounds();
    }

    private void LoadSounds()
    {
        // BGM
        LoadSound("Sound/BackGround", out backGroundSoundClipList, out backGroundSoundClipDic);

        // 이펙트 사운드
        LoadSound("Sound/SFX", out sfxSoundClipList, out sfxSoundClipDic);

        // 남자 사령관 대사
        LoadSound("Sound/DialogueRecording/Man", out manDialogueRecordingList, out manDialogueRecordingDic);

        // 여자 사령관 대사
        LoadSound("Sound/DialogueRecording/Woman", out womanDialogueRecordingList, out womanDialogueRecordingDic);
    }

    private void LoadSound(string path, out List<AudioClip> clips, out Dictionary<string, AudioClip> clipsDic)
    {
        clips = Resources.LoadAll<AudioClip>(path).ToList();
        clipsDic = clips.ToDictionary(clip => clip.name);
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
