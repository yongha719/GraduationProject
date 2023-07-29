using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3Card : MasicCard
{
    public override void Ability()
    {
        TurnManager.Instance.ShouldSummonCopy = true;
    }
}
