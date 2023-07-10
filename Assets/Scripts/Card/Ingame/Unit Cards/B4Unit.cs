using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B4Unit : UnitCard
{


    private const int ADDITIONAL_DAMAGE = 1;

    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Instantiate(illustAppearEffect).GetComponent<CharacterProduction>().characterType = ECharacterType.Hanseorin;
        SoundManager.Instance.PlaySFXSound(ECharacterType.Hanseorin.ToString());
    }


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
