using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1Unit : UnitCard
{
    public void BasicAttack(UnitCard card)
    {
        if (CardManager.Instance.EnemyUnits.Count == 1)
        {
            // 테스트로 만들고 있음
            card.Hit(1);
        }
        else
        {

        }

    }
}
