using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardManager
{
    private static List<UnitCard> PlayerUnits = new List<UnitCard>(10);
    private static List<UnitCard> EnemyUnits = new List<UnitCard>(10);

    public static string Name;

    [Tooltip("내 덱")]
    private static List<Card> myDeck = new List<Card>();
    public static List<Card> MyDeck
    {
        get => myDeck;

        set
        {
            myDeck = value;
        }
    }

    /// <summary> Player Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddPlayerUnit(this UnitCard card)
    {
        PlayerUnits.Add(card);
    }

    /// <summary> Enemy Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddEnemyUnit(this UnitCard card)
    {
        EnemyUnits.Add(card);
    }

    public static List<UnitCard> GetPlayerUnitCards(this UnitCard card) => PlayerUnits;
    public static List<UnitCard> GetEnemyUnitCards(this UnitCard card) => EnemyUnits;

    public static void RemovePlayerUnit(this UnitCard card)
    {
        if (PlayerUnits.Contains(card))
            PlayerUnits.Remove(card);
    }


    /// <summary>  </summary>
    public static void RemoveEnemyUnit(this UnitCard card)
    {
        if (EnemyUnits.Contains(card))
            EnemyUnits.Remove(card);
    }

    public static void CardSpawnEvent(this Action<UnitCard> action)
    {
        action?.Invoke(EnemyUnits.Last());
    }

    public static void SerializeUnitCards(this PhotonStream stream)
    {
        // 적과 플레이어는 반대로 받아와야 하기 때문에 메서드를 만들었음
        stream.Serialize(PlayerUnits, EnemyUnits);
        stream.Serialize(EnemyUnits, PlayerUnits);
    }
}
