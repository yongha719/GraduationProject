using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class M5Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        CardManager.Instance.UseMineMasic = true;   
    }
}
