using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B4Unit : UnitCard
{
    private const int ADDITIONAL_DAMAGE = 1;

    protected override void BasicAttack(UnitCard enemyCard)
    {
        base.BasicAttack(enemyCard);

        if (enemyCard.isHypothermic)
        {
            enemyCard.Hit(ADDITIONAL_DAMAGE);
        }
        else
        {
            enemyCard.isHypothermic = true;
        }
    }
}
