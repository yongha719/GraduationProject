using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A1Unit : UnitCard, IUnitCardAction
{
    protected override void Start()
    {
        base.Start();

        unitCardAction = this;
    }

    public void BasicAttack(UnitCard card)
    {
        print($"{nameof(A1Unit)} : Attack");


        var enemyList = CardManager.Instance.EnemyUnitCard;
        var index = enemyList.IndexOf(card);

        if (enemyList.Count != 1)
        {
            UnitCard leftcard = null;
            UnitCard rightcard = null;

            if (index > 0)
                leftcard = enemyList[index - 1];

            if (enemyList.Count - 1 > index)
                rightcard = enemyList[index + 1];

            print(enemyList.Count);


            print($"card index is {index}");

            if (leftcard != null)
            {
                print("left Card");
                leftcard.Hit(Damage);
            }

            if (rightcard != null)
            {
                print("right card");
                rightcard.Hit(Damage);
            }
        }

        card.Hit(Damage, Hit);
    }

    public void SpecialAbility()
    {
        if (HasSpecialAbility == false) return;


    }
}