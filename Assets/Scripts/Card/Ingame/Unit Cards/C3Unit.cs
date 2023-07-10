using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Photon.Pun;
using UnityEngine;

public class C3Unit : UnitCard
{
    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Instantiate(illustAppearEffect).GetComponent<CharacterProduction>().characterType = ECharacterType.CleaningRobot;
    }

    // 한번 버텼는지 체크
    private bool isSurvivedOnce = false;

    public override void Hit(int damage)
    {
        // 피 1 남기고 한번 버틸 수 있음
        if (isSurvivedOnce == false && Hp <= damage)
        {
            Hp = 1;
            isSurvivedOnce = true;
        }
        else
            base.Hit(damage);
    }

    /// <summary>공격받고 상대한테 데미지도 입힘</summary>
    /// <param name="hitAction">자기가 공격받고 나서 상대가 데미지입는 액션</param>
    public override void Hit(int damage, Action<int> hitAction)
    {
        Hit(damage);

        hitAction(Damage);
    }
}