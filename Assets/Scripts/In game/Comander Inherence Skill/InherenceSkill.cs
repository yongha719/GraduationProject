using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public enum EInherenceSkillType
{
    None,
    PhotonShield = 0, //광자 보호막
    PistolShot, //권총 발사
    Barrier, //방벽
    EmergencyCall, //긴급호출
    ChangeMood, //분위기 전환
    FastCharger, //급속충전기
    End,
}

public abstract class InherenceSkill : MonoBehaviourPun
{
    public abstract EInherenceSkillType InherenceSkillType { get; }

    [SerializeField] private Button useSkillButton;

    private Sprite OriginSprite;
    private Sprite disableSprite;
    
    protected Action enableAttackCall;
    protected bool useSkill;

    private void Start()
    {
        useSkillButton = GetComponent<Button>();
        
        useSkillButton.onClick.AddListener(Skill);

        useSkillButton.image.sprite = Resources.Load<Sprite>($"InherenceSkill/{InherenceSkillType}_Skill");
        disableSprite = Resources.Load<Sprite>($"InherenceSkill/Disable_Skill");

        enableAttackCall = () => 
            TurnManager.Instance.ExecuteAfterTurn(1,
                beforeTurnCall: () =>
                {
                    useSkillButton.image.sprite = disableSprite;
                    print($"Can't Attack :  {useSkill}");
                },
                afterTurnCall: () =>
                {
                    useSkillButton.image.sprite = OriginSprite;
                    print($"Can Attack : {useSkill}");
                });
    }

    protected abstract void Skill();
}