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
public class CardData
{
    [Tooltip("이름"), SerializeField]
    private string name;
    public string Name => name;


    [Tooltip("공격력"), SerializeField]
    private int power;
    public int Power => power;


    [Tooltip("체력"), SerializeField]
    private int hp;
    public int Hp => hp;


    [Tooltip("코스트"), SerializeField]
    private int cost;
    public int Cost => cost;


    [Tooltip("치명타 확률"), SerializeField]
    private int criticalPercentage = -1;
    public int CriticalPercentage => criticalPercentage;


    [Tooltip("치명타 공격력"), SerializeField]
    private int criticalPower = 0;
    public int CriticalPower => criticalPower;


    [Tooltip("기본 공격 설명"), TextArea, SerializeField]
    private string basicAttackExplain;
    public string BasicAttackExplain => basicAttackExplain;


    [Tooltip("특성"), SerializeField]
    private CardAttributeType cardAttributeType;
    public CardAttributeType CardAttributeType => cardAttributeType;


    [Tooltip("특수 공격 설명"), TextArea, SerializeField]
    private string specialAttackExplain;
    public string SpecialAttackExplain => specialAttackExplain;


    [Tooltip("등급"), TextArea, SerializeField]
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
        for (int i = 0; i < (int)CardDataIndex.End; i++)
        {
            this[i] = data[i];
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

    public CardData Copy()
    {
        return new CardData(this);
    }
}
