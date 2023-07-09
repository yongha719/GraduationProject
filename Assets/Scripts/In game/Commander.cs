using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Commander : MonoBehaviourPun, IPunObservable
{
    private int hp;

    public int Hp
    {
        get => hp;

        set
        {
            if (value <= 0)
                isWin = false;

            hp = value;
        }
    }

    [SerializeField] private bool isEnemy = false;

    private bool isWin = true;

    [SerializeField] private EInherenceSkillType inherenceSkillType;

    [SerializeField] private GameObject inherenceSkillParent;

    [SerializeField] private Image commanderImage;

    public bool CanAttackThis => CardManager.Instance.PlayerUnitCards.Count == 0;

    private void Start()
    {
        inherenceSkillType = GameManager.Instance.CommanderInherenceSkillType;

        commanderImage = GetComponent<Image>();

        TurnManager.Instance.FirstTurnAction += Init;

        if (isEnemy)
            return;

        var skill = inherenceSkillParent.AddComponent(Type.GetType($"{inherenceSkillType}Skill"));

        if (skill is InherenceSkill inherenceSkill)
        {
            inherenceSkill.isEnemy = isEnemy;
        }
    }

    private void Init()
    {
        string spritePath;

        if (PhotonNetwork.IsMasterClient)
            spritePath = isEnemy ? "Commander/Enemy_Female_Commader" : "Commander/Player_Male_Commader";
        else
            spritePath = isEnemy ? "Commander/Enemy_Male_Commader" : "Commander/Player_Female_Commader";

        commanderImage.sprite = Resources.Load<Sprite>(spritePath);
    }

    [PunRPC]
    private void GameOver()
    {
        if (isWin)
        {
            print("이겼다");
        }
        else
        {
            print("힝 졌다");
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else if (stream.IsReading && isEnemy)
        {
        }
    }
}