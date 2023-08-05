using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M8Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        CardManager.Instance.HandleCards(unitCard =>
        {
            unitCard.Cleanse();
        });
    }
}
