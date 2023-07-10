using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A1Unit : UnitCard
{
    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Instantiate(illustAppearEffect).GetComponent<CharacterProduction>().characterType = ECharacterType.Baekyura;
        SoundManager.Instance.PlaySFXSound(ECharacterType.Baekyura.ToString());
    }

    protected override void BasicAttack(UnitCard enemyCard)
    {
        var enemyList = CardManager.Instance.EnemyUnitCards;

        var index = enemyList.IndexOf(enemyCard);

        if (enemyList.Count != 1)
        {
            if (index > 0)
                enemyList[index - 1].Hit(1);

            if (enemyList.Count - 1 > index)
                enemyList[index + 1].Hit(1);
        }

        base.BasicAttack(enemyCard);
    }
}