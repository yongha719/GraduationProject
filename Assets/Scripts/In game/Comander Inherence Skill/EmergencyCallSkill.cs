using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmergencyCallSkill : InherenceSkill
{
    public override EInherenceSkillType InherenceSkillType => EInherenceSkillType.EmergencyCall;
    
    protected override void Skill()
    {
        var C2 = CardManager.Instance.CardDraw("C2", isPlayeDraw: false, setParentAsDeck: false);
        
        C2.GetComponent<C2Card>().SetStat(1,1);
    }
}
