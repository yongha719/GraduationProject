using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A1Unit : UnitCard
{
    protected override void BasicAttack(UnitCard card)
    {
        print($"{nameof(A1Unit)} : Attack");

        var enemyList = CardManager.Instance.EnemyUnitCards;

        var index = enemyList.IndexOf(card);

        if (enemyList.Count != 1)
        {
            if (index > 0)
                enemyList[index - 1].Hit(Damage);

            if (enemyList.Count - 1 > index)
                enemyList[index + 1].Hit(Damage);
        }

        base.BasicAttack(card);
    }
}