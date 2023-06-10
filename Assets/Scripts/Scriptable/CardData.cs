using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
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
    Damage,
    End,
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
public struct CardData
{
    [Tooltip("이름"), SerializeField]
    private string name;
    public string Name => name;


    [Tooltip("공격력"), SerializeField]
    private int power;
    public int Power
    {
        get => power;
        set => power = value;
    }


    [Tooltip("체력"), SerializeField]
    private int hp;
    public int Hp
    {
        get => hp;
        set => hp = value;
    }


    [Tooltip("코스트"), SerializeField]
    private int cost;
    public int Cost
    {
        get => cost;
        set => cost = value;
    }


    [Tooltip("치명타 확률"), SerializeField]
    private int criticalPercentage;
    public int CriticalPercentage
    {
        get => criticalPercentage;
        set => criticalPercentage = value;
    }


    [Tooltip("치명타 공격력"), SerializeField]
    private int criticalPower;
    public int CriticalPower
    {
        get => criticalPower;
        set => criticalPower = value;
    }

    [Space]
    
    [Tooltip("기본 공격 설명"), TextArea, SerializeField]
    private string basicAttackExplain;
    public string BasicAttackExplain => basicAttackExplain;

    [Space]

    [Tooltip("특성"), SerializeField]
    private CardAttributeType cardAttributeType;
    public CardAttributeType CardAttributeType
    {
        get => cardAttributeType;
        set => cardAttributeType = value;
    }

    [Space]

    [Tooltip("특수 공격 설명"), TextArea, SerializeField]
    private string specialAttackExplain;
    public string SpecialAttackExplain => specialAttackExplain;


    [Tooltip("등급"), SerializeField]
    private string cardRating;
    public string CardRating => cardRating;


    /// <summary> 크리티컬까지 계산된 데미지 </summary>
    public int Damage
    {
        get
        {
            if (UnityEngine.Random.Range(0, 100) <= criticalPercentage)
                return power + criticalPower; // 크리티컬 데미지까지 추가된 공격력
            else
                return power;
        }
    }


    public CardData(CardData data)
    {
        name = data.name;
        power = data.power;
        hp = data.hp;
        cost = data.cost;
        criticalPercentage = data.criticalPercentage;
        criticalPower = data.criticalPower;
        basicAttackExplain = data.basicAttackExplain;
        cardAttributeType = data.cardAttributeType;
        specialAttackExplain = data.specialAttackExplain;
        cardRating = data.cardRating;
    }

    public CardData(string[] data)
    {
        name = data[0];
        power = int.Parse(data[1]);
        hp = int.Parse(data[2]);
        cost = int.Parse(data[3]);
        criticalPercentage = int.Parse(data[4]);
        criticalPower = int.Parse(data[5]);
        basicAttackExplain = data[6];
        Enum.TryParse(data[7], out cardAttributeType);
        specialAttackExplain = data[8];
        cardRating = data[9].Replace("\r", "");
    }

    /// <summary> 혹시나 쓰지 않을까 싶어서 만들어놨음 </summary>
    public string this[int i]
    {
        get => i switch
        {
            0 => Name,
            1 => power.ToString(),
            2 => hp.ToString(),
            3 => Cost.ToString(),
            4 => criticalPercentage.ToString(),
            5 => criticalPower.ToString(),
            6 => basicAttackExplain,
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
                    name = value;
                    break;
                case 1:
                    power = int.Parse(value);
                    break;
                case 2:
                    hp = int.Parse(value);
                    break;
                case 3:
                    cost = int.Parse(value);
                    break;
                case 4:
                    criticalPercentage = int.Parse(value);
                    break;
                case 5:
                    criticalPower = int.Parse(value);
                    break;
                case 6:
                    basicAttackExplain = value;
                    break;
                case 7:
                    if (value.IsNullOrEmpty() == false)
                        cardAttributeType = (CardAttributeType)Enum.Parse(typeof(CardAttributeType), value);
                    break;
                case 8:
                    specialAttackExplain = value;
                    break;
                case 9:
                    cardRating = value.Replace("\r", "");
                    break;
                default:
                    break;
            }
        }
    }
}
