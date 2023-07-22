using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Card : MasicCard
{
    public override int Cost => 4;

    private const int DAMAGE = 6;

    public override void Ability(UnitCard enemyCard)
    {
        enemyCard.Hit(DAMAGE);
    }
}
