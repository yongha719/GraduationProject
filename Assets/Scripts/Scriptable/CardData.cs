using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData", order = int.MinValue)]
public class CardData : ScriptableObject
{
    [Tooltip("이름")]
    public string Name;
    [Tooltip("설명")]
    public string Explain;
    [Tooltip("코스트")]
    public int Cost;

    /// <summary> 계산된 데미지 </summary>
    public int Damage
    {
        get
        {
            if (Random.Range(0, 100) <= CriticalPercentage)
                return Power + CriticalPower; // 크리티컬 데미지까지 추가된 공격력
            else
                return Power;
        }
    }

    [Tooltip("공격력")]
    public int Power;
    [Tooltip("체력")]
    public int Hp;
    [Tooltip("치명타 확률")]
    public int CriticalPercentage = -1;
    [Tooltip("치명타 공격력")]
    public int CriticalPower = 0;
}
