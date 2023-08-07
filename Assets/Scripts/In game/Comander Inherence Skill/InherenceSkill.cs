using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
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
    
    protected virtual WaitUntil waitClick { get; }

    public bool isEnemy;

    [SerializeField] private Button useSkillButton;

    private Sprite OriginSprite;
    private Sprite disableSprite;

    protected Action enableAttackCall;
    protected bool useSkill;

    protected virtual void Start()
    {
        useSkillButton = GetComponent<Button>();

        OriginSprite = Resources.Load<Sprite>($"InherenceSkill/{InherenceSkillType}_Skill");
        disableSprite = Resources.Load<Sprite>($"InherenceSkill/Disable_Skill");

        useSkillButton.onClick.AddListener(Skill);
        useSkillButton.image.sprite = OriginSprite;

        // 스킬 버튼을 비활성화하고 1턴 뒤에 다시 활성화
        enableAttackCall = () =>
            TurnManager.Instance.ExecuteAfterTurn(1,
                beforeTurnCall: () =>
                {
                    useSkillButton.interactable = false;
                    useSkillButton.image.sprite = disableSprite;
                    print($"Can't Attack :  {useSkill}");
                },
                afterTurnCall: () =>
                {
                    useSkillButton.interactable = true;
                    useSkillButton.image.sprite = OriginSprite;
                    print($"Can Attack : {useSkill}");
                });
    }

    protected abstract void Skill();
}