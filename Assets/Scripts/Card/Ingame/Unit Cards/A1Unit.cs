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
        card.Hit(CardData.Damage, Hit);
        
        var enemyList = CardManager.Instance.EnemyUnitCardInfos;
            
        if (enemyList.Count == 1)
            return;

        var index = enemyList.IndexOf(card);

        if (index > 0)
        {
            enemyList[index - 1].Hit(CardData.Damage);
        }

        if (enemyList.Count - 1 > index)
        {
            enemyList[index + 1].Hit(CardData.Damage);
        }
    }

    public void SpecialAbility()
    {
        if(HasSpecialAbility == false) return;
        
        
    }
}
