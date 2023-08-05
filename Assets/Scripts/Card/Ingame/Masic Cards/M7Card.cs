using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드를 3장 뽑습니다
/// </summary>
public class M7Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        CardManager.Instance.CardDraw(3);
    }
}
