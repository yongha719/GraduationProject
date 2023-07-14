using System;
using UnityEngine;

public enum MasicAbilityTarget
{
    Field,
    Ally,
    Enemy,
}

[Serializable]
public class MasicCardData
{
    [Tooltip("카드 이름")]
    private string name;

    public string Name => name;

    [Tooltip("타겟 타입")]
    private MasicAbilityTarget masicAbilityTarget;

    public MasicAbilityTarget MasicAbilityTarget => masicAbilityTarget;

    [Tooltip("카드 등급")]
    private string cardRating;

    public string CardRating => cardRating;

    public MasicCardData(MasicCardData data)
    {
        name = data.name;
        masicAbilityTarget = data.masicAbilityTarget;
        cardRating = data.cardRating;
    }

    public MasicCardData(string[] data)
    {
        name = data[0];
        Enum.TryParse(data[1], out masicAbilityTarget);
        cardRating = data[2];
    }
}
