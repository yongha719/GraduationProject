using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2Unit : UnitCard
{
    // 맹독 공격이 특성임

    protected override void BasicAttack(UnitCard enemyCard)
    {
        base.BasicAttack(enemyCard);

        // 맹독 공격
        enemyCard.Hit(1);

        // 2턴에 걸쳐 공격함
        TurnManager.Instance.ExecuteAfterTurn(1, () =>
        {
            enemyCard.Hit(1);
            print("Attack");
        });
    }
}
