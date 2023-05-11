using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;

[Serializable]
public enum CardDataIndex
{
    Name = 0,
    Power,
    Hp,
    Cost,
    CriticalPercentage,
    CriticalPower,
    BasicAttackExplain,
    CardAttributeType,
    SpecialAttackExplain,
    CardRating,
    Damage
}

[Serializable]
public enum CardAttributeType
{
    None,
    Taunt,                      // 도발
    Charge,                     // 속공
    Poison,                     // 맹독
    LastDitchEffort,            // 최후의 발악
}


[Serializable]
public class CardData
{
    [Tooltip("이름")]
    public string Name;

    [Tooltip("공격력")]
    public int Power;

    [Tooltip("체력")]
    public int Hp;

    [Tooltip("코스트")]
    public int Cost;

    [Tooltip("치명타 확률")]
    public int CriticalPercentage = -1;

    [Tooltip("치명타 공격력")]
    public int CriticalPower = 0;

    [Tooltip("기본 공격 설명"), TextArea]
    public string BasicAttackExplain;

    [Tooltip("특성")]
    public CardAttributeType CardAttributeType;

    [Tooltip("특수 공격 설명"), TextArea]
    public string SpecialAttackExplain;

    [Tooltip("등급"), TextArea]
    public string CardRating;

    /// <summary> 크리티컬 확률까지 계산된 데미지 </summary>
    public int Damage
    {
        get
        {
            if (UnityEngine.Random.Range(0, 100) <= CriticalPercentage)
                return Power + CriticalPower; // 크리티컬 데미지까지 추가된 공격력
            else
                return Power;
        }
    }

    public CardData(string[] data)
    {
        for (int i = 0; i < data.Length; i++)
            this[i] = data[i];
    }

    /// <summary> 혹시나 쓰지 않을까 싶어서 만들어놨음 </summary>
    public string this[int i]
    {
        get => i switch
        {
            0 => Name,
            1 => Power.ToString(),
            2 => Hp.ToString(),
            3 => Cost.ToString(),
            4 => CriticalPercentage.ToString(),
            5 => CriticalPower.ToString(),
            6 => BasicAttackExplain,
            7 => CardAttributeType.ToString(),
            8 => SpecialAttackExplain,
            9 => CardRating.ToString(),
            10 => Damage.ToString(),
            _ => throw new IndexOutOfRangeException("Card Data Indexer Exception")
        };

        set
        {
            switch (i)
            {
                case 0:
                    Name = value;
                    break;
                case 1:
                    Power = int.Parse(value);
                    break;
                case 2:
                    Hp = int.Parse(value);
                    break;
                case 3:
                    Cost = int.Parse(value);
                    break;
                case 4:
                    CriticalPercentage = int.Parse(value);
                    break;
                case 5:
                    CriticalPower = int.Parse(value);
                    break;
                case 6:
                    BasicAttackExplain = value;
                    break;
                case 7:
                    if (value.IsNullOrEmpty() == false)
                        CardAttributeType = (CardAttributeType)Enum.Parse(typeof(CardAttributeType), value);
                    break;
                case 8:
                    SpecialAttackExplain = value;
                    break;
                case 9:
                    CardRating = value.Replace("\r", "");
                    break;
                default:
                    throw new System.IndexOutOfRangeException("Card Data Indexer Exception");
            }
        }
    }
}
