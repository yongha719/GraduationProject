using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class A2Unit : UnitCard
{
    [SerializeField, Tooltip("해킹할건지 공격할건지 선택하는 UI")]
    private GameObject ChoiceUI;

    public event Action AttackOrHack = () => { };

    private UnitCard curHackedUnitCard;

    protected override void Start()
    {
        base.Start();

        ChoiceUI = Resources.Load<GameObject>("Cards/A2_UI");

        ChoiceUI = Instantiate(ChoiceUI, Vector3.zero, Quaternion.identity, transform.parent.parent.parent);
        ChoiceUI.SetActive(false);

        var choiceUI = ChoiceUI.GetComponent<A2ChoiceUI>();

        choiceUI.Init(this, Attack, Hacking);
    }

    protected override void BasicAttack(UnitCard enemyCard)
    {
        curHackedUnitCard = enemyCard;
        
        ChoiceUI.SetActive(true);
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