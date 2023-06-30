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
        ChoiceUI = Instantiate(ChoiceUI, Vector3.zero, Quaternion.identity, transform.parent);
        ChoiceUI.SetActive(false);

        var choiceUI = ChoiceUI.GetComponent<A2ChoiceUI>();

        choiceUI.Init(this, Attack, Hacking);

        base.Start();
    }

    protected override void BasicAttack(UnitCard enemyCard)
    {
        curHackedUnitCard = enemyCard;
        
        ChoiceUI.SetActive(true);
    }

    private void Attack()
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

    private IEnumerator HackingCoroutine()
    {
        yield return TurnManager.WaitTurn(1);
    }

    // 해킹하고 상대 유닛 통제
    // N턴뒤에 해킹이 풀리는거까지
    private void HackingCount(bool myTurn, int turnCount)
    {
        if (myTurn == false)
            return;

        if (turnCount != 0)
            turnCount--;
        else
        {
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Destroy(ChoiceUI);
    }
}