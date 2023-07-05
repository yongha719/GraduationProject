using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMasicCardSubject
{
    MasicAbilityTarget AbilityTarget { get; }

    void Ability();
    
    void Ability(UnitCard card);
}
