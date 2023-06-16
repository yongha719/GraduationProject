using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1Unit : MonoBehaviourPun, IUnitCardAction
{
    private UnitCard _unitCard;
    
    public void BasicAttack(UnitCard card)
    {
        card.Hit(100);
        
        var enemyList = CardManager.Instance.EnemyUnitCardInfos;
            
        if (enemyList.Count == 1)
            return;

        var index = enemyList.IndexOf(card);

        if (index > 1)
        {
            enemyList[index - 1].Hit(_unitCard);
        }
        
        
    }

    public void SpecialAbility()
    {
        throw new NotImplementedException();
    }
}
