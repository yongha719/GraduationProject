using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 랜덤 확률로 버프나 디버프를 줌
/// 확률은 다 똑같음
/// 공격력 +- 1,2 / 생명력 +- 1,2/ 저체온증/ 도발
/// </summary>
public class M9Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        int randomValue = Random.Range(0, 10);

        switch (randomValue)
        {
            case 0 or 1:
                card.Hp += randomValue + 1;
                break;
            case 2 or 3:
                card.Hp -= randomValue - 1;
                break;
            case 4 or 5:
                card.CardData.Power += randomValue - 3;
                break;
            case 6 or 7:
                card.CardData.Power -= randomValue - 5;
                break;
            case 8:
                card.isHypothermic = true;
                break;
            case 9:
                card.CardData.UnitCardSpecialAbilityType = UnitCardSpecialAbilityType.Taunt;
                break;
        }
    }
}
