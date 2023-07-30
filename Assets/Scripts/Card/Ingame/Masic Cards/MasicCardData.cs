using System;
using UnityEngine;

[Serializable]
public enum MasicAbilityTarget
{
    Field,
    Ally,
    Enemy,
}

[Serializable]
public class MasicCardData : CardData
{
    [Tooltip("타겟 타입"), SerializeField]
    private MasicAbilityTarget masicAbilityTarget;

    public MasicAbilityTarget MasicAbilityTarget => masicAbilityTarget;
    
    public MasicCardData(){}
    
    public MasicCardData(MasicCardData data)
    {
        Name = data.Name;
        masicAbilityTarget = data.masicAbilityTarget;
        CardRating = data.CardRating;
    }

    public override void Init(string[] data)
    {
        Name = data[0];
        Enum.TryParse(data[1], out masicAbilityTarget);
        CardRating = data[2];
    }

    public override CardData Copy()
    {
        return new MasicCardData(this);
    }
}
