using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2Unit : UnitCard
{
    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        SoundManager.Instance.PlaySFXSound("Hanyerin");
    }

    // 맹독 공격이 특성임
    protected override void BasicAttack(UnitCard enemyCard)
    {
        base.BasicAttack(enemyCard);

        var deadlyPoisonProduction = enemyCard.gameObject.AddComponent<DeadlyPoisonProduction>();

        // 맹독 공격
        enemyCard.Hit(1);

        // 2턴에 걸쳐 공격함
        TurnManager.Instance.ExecuteAfterTurn(1, () =>
        {
            enemyCard.Hit(1);

            Destroy(deadlyPoisonProduction);
            print("Attack");
        });
    }
}
