using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData", order = int.MinValue)]
public class CardData : ScriptableObject
{
    public string Name;
    public int Cost;
    public int Power;
    public int Hp;
}
