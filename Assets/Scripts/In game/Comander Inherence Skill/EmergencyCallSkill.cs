using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmergencyCallSkill : InherenceSkill
{
    public override EInherenceSkillType InherenceSkillType => EInherenceSkillType.EmergencyCall;
    
    protected override void Skill()
    {
        var C2 = CardManager.CardDrawCall("C2", isTest: false, isUnit: true, setParentAsDeck: false);
        
        C2.GetComponent<C2Unit>().SetStat(1,1);
    }
}
