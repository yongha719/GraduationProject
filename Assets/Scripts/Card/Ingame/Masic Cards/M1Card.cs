using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1Card : MasicCard
{
    private const int MISSILE_DAMAGE = 1;

    public override void Ability()
    {
        var enemyList = CardManager.Instance.EnemyUnitCards;

        var randomEnemy1 = enemyList[Random.Range(0, enemyList.Count)] as UnitCard;
        // 미사일 연출 추가
        randomEnemy1.Hit(MISSILE_DAMAGE);

        var randomEnemy2 = enemyList[Random.Range(0, enemyList.Count)];
        // 미사일 연출 추가
        randomEnemy2.Hit(MISSILE_DAMAGE);
    }
}