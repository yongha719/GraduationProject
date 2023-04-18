using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData", order = int.MinValue)]
public class CardData : ScriptableObject
{
    [Tooltip("�̸�")]
    public string Name;
    [Tooltip("����")]
    public string Explain;
    [Tooltip("�ڽ�Ʈ")]
    public int Cost;

    /// <summary> ���� ������ </summary>
    public int Damage
    {
        get
        {
            if (Random.Range(0, 100) <= CriticalPercentage)
                return Power + CriticalPower; // ũ��Ƽ�� ���������� �߰��� ���ݷ�
            else
                return Power;
        }
    }

    [Tooltip("���ݷ�")]
    public int Power;
    [Tooltip("ü��")]
    public int Hp;
    [Tooltip("ġ��Ÿ Ȯ��")]
    public int CriticalPercentage = -1;
    [Tooltip("ġ��Ÿ ���ݷ�")]
    public int CriticalPower = 0;
}
