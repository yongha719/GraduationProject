using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WebSocketSharp;

[Serializable]
public enum UnitCardSpecialAbilityType
{
    None,
    Taunt, // 도발
    Charge, // 속공
    Poison, // 맹독
    LastDitchEffort, // 최후의 발악
}


[Serializable]
public sealed class UnitCardData : CardData
{
    [Tooltip("공격력"), SerializeField]
    private int power;

    public int Power
    {
        get => power;
        set => power = value;
    }


    [Tooltip("체력"), SerializeField]
    private int hp = -1;

    public int Hp
    {
        get => hp;
        set => hp = value;
    }


    [Tooltip("치명타 확률"), SerializeField]
    private int criticalPercentage;

    public int CriticalPercentage
    {
        get => criticalPercentage;
        set => criticalPercentage = value;
    }


    [Tooltip("치명타 공격력")]
    public int CriticalPower { get; private set; }

    [Tooltip("기본 공격 설명")]
    public string BasicAttackExplain { get; private set; }

    [Space]
    [Tooltip("특성"), SerializeField]
    private UnitCardSpecialAbilityType unitCardSpecialAbilityType;

    public UnitCardSpecialAbilityType UnitCardSpecialAbilityType
    {
        get => unitCardSpecialAbilityType;
        set => unitCardSpecialAbilityType = value;
    }

    [Space]
    [Tooltip("특수 공격 설명"), TextArea, SerializeField]
    private string specialAttackExplain;

    public string SpecialAttackExplain
    {
        get => specialAttackExplain;
        set => specialAttackExplain = value;
    }


    /// <summary> 크리티컬까지 계산된 데미지 </summary>
    public int Damage
    {
        get
        {
            if (UnityEngine.Random.Range(0, 100) <= criticalPercentage)
                return power + CriticalPower; // 크리티컬 데미지까지 추가된 공격력

            return power;
        }
    }

    public UnitCardData()
    {
    }

    public UnitCardData(UnitCardData data)
    {
        Name = data.Name;
        power = data.power;
        hp = data.hp;
        Cost = data.Cost;
        criticalPercentage = data.criticalPercentage;
        CriticalPower = data.CriticalPower;
        BasicAttackExplain = data.BasicAttackExplain;
        unitCardSpecialAbilityType = data.unitCardSpecialAbilityType;
        specialAttackExplain = data.specialAttackExplain;
        CardRating = data.CardRating;
    }

    public override void Init(string[] data)
    {
        Name = data[0];
        power = int.Parse(data[1]);
        hp = int.Parse(data[2]);
        Cost = int.Parse(data[3]);
        criticalPercentage = int.Parse(data[4]);
        CriticalPower = int.Parse(data[5]);
        BasicAttackExplain = data[6];
        Enum.TryParse(data[7], out unitCardSpecialAbilityType);
        specialAttackExplain = data[8];
        CardRating = data[9].Replace("\r", "");
    }

    public override CardData Copy()
    {
        return new UnitCardData(this);
    }
}