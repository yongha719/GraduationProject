using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// A2 유닛카드는 공격과 해킹 중 선택할 수 있어서
// UI를 만들어놨음
public class A2ChoiceUI : MonoBehaviour
{
    private A2Unit Card;

    private Action AttackCall;
    private Action HackingCall;

    [SerializeField] private Button attackButton;
    [SerializeField] private Button hackingButton;

    
    public void Init(A2Unit card, Action attackCall, Action hackingCall)
    {
        Card = card;
        AttackCall = attackCall;
        HackingCall = hackingCall;
    }

    private void Start()
    {
        attackButton.onClick.AddListener(() => AttackCall());
        hackingButton.onClick.AddListener(() => HackingCall());
    }
}
