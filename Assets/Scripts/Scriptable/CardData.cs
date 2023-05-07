using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData", order = int.MinValue)]
public class CardData
{
    [Tooltip("등급")]
    public string CardRating;
    [Tooltip("이름")]
    public string Name;
    [Tooltip("기본 공격 설명")]
    public string BasicAttackExplain;
    [Tooltip("특수 공격 설명")]
    public string SpecialAttackExplain;
    [Tooltip("코스트")]
    public int Cost;

    /// <summary> 계산된 데미지 </summary>
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

    BoxCollider2D dad;

    [Tooltip("공격력")]
    public int Power;
    [Tooltip("체력")]
    public int Hp;
    [Tooltip("치명타 확률")]
    public int CriticalPercentage = -1;
    [Tooltip("치명타 공격력")]
    public int CriticalPower = 0;

    public CardData(string[] data)
    {
        Name = data[0];
        Power = int.Parse(data[1]);
        Hp = int.Parse(data[2]);
        Cost = int.Parse(data[3]);
        CriticalPercentage = int.Parse(data[4]);
        CriticalPower = int.Parse(data[5]);
        BasicAttackExplain = data[6];
        SpecialAttackExplain = data[7];
        CardRating = data[8];
    }
}
