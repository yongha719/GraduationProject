using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum A2AttackType
{
    BasicAttack,
    Hacking
}

public class A2Unit : UnitCard
{
    [SerializeField, Tooltip("해킹할건지 공격할건지 선택하는 UI")]
    private GameObject ChoiceUI;

    public A2AttackType AttackType
    {
        set
        {
            if (value == A2AttackType.BasicAttack)
                attackTypeAction = Attack;
            else
                attackTypeAction = Hacking;
        }
    }

    private Action attackTypeAction;

    private UnitCard curHackedUnitCard;


    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Instantiate(illustAppearEffect).GetComponent<CharacterProduction>().characterType = ECharacterType.Yooeunha;
    }

    protected override void Start()
    {
        base.Start();

        ChoiceUI = Resources.Load<GameObject>("Cards/A2_UI");

        ChoiceUI = Instantiate(ChoiceUI, Vector3.zero, Quaternion.identity, rect);
        ChoiceUI.SetActive(false);

        OnFieldChangeAction += () => ChoiceUI.SetActive(true);

        var choiceUI = ChoiceUI.GetComponent<A2ChoiceUI>();

        choiceUI.Init(this);
    }

    protected override void BasicAttack(UnitCard enemyCard)
    {
        curHackedUnitCard = enemyCard;
    }

    protected override void Attack()
    {
        base.BasicAttack(curHackedUnitCard);
    }

    // 해킹
    public void Hacking()
    {
        TurnManager.Instance.ExecuteAfterTurn(1,
            beforeTurnCall: () => curHackedUnitCard.IsHacked = true,
            afterTurnCall: () => curHackedUnitCard.IsHacked = false);
    }

    protected override void OnDestroy()
    {
        Destroy(ChoiceUI);

        base.OnDestroy();
    }
}