using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Commander : MonoBehaviourPun
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

    [SerializeField] private bool isEnemy;

    private bool isWin = true;

    [SerializeField] private EInherenceSkillType inherenceSkillType;

    public bool CanAttackThis => CardManager.Instance.PlayerUnitCards.Count == 0;

    private void Start()
    {
        inherenceSkillType = GameManager.Instance.CommanderInherenceSkillType;
        
        transform.GetChild(0).gameObject.AddComponent(Type.GetType($"{inherenceSkillType}Skill"));
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
}