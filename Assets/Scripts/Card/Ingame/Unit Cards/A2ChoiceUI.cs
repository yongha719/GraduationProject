using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// A2 유닛카드는 공격과 해킹 중 선택할 수 있어서
// UI를 만들어놨음
public class A2ChoiceUI : MonoBehaviour
{
    private A2Card Card;

    private Action AttackCall;
    private Action HackingCall;

    private bool selectType = false;

    [SerializeField] private Button attackTypeSelectButton;

    [SerializeField] private GameObject SelectAttackObj;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button hackingButton;

    RectTransform rect;
    
    public void Init(A2Card card)
    {
        Card = card;
    }

    private void Start()
    {
        attackTypeSelectButton.onClick.AddListener(() =>
        {
            SelectAttackObj.SetActive(!selectType);
            selectType = !selectType;
        });

        attackButton.onClick.AddListener(() =>
        {
            print("A2 Select Attack");
            attackTypeSelectButton.image.sprite = attackButton.image.sprite;
            Card.AttackType = A2AttackType.BasicAttack;
        });

        hackingButton.onClick.AddListener(() =>
        {
            print("A2 Select Hacking");
            attackTypeSelectButton.image.sprite = hackingButton.image.sprite;
            Card.AttackType = A2AttackType.Hacking;
        });

        rect = transform as RectTransform;
        rect.anchoredPosition3D = Vector3.zero;
    }

    private void Update()
    {
        rect.anchoredPosition3D = Vector3.zero;
        rect.localRotation = Quaternion.identity;
    }
}
