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
    private static List<string> myDeck = new List<string>();
    public static List<string> MyDeck
    {
        get => myDeck;

        set
        {
            myDeck = value;
        }
    }

    /// <summary> Player Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddPlayerUnit(this UnitCard card) => PlayerUnits.Add(card);

    /// <summary> Enemy Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddEnemyUnit(this UnitCard card) => EnemyUnits.Add(card);

     /// <summary> Player의 Unit Card들을 반환 </summary>
    public static List<UnitCard> GetPlayerUnitCards(this UnitCard card) => PlayerUnits;

     /// <summary> Enemy의 Unit Card들을 반환 </summary>
    public static List<UnitCard> GetEnemyUnitCards(this UnitCard card) => EnemyUnits;

    public static void RemoveUnit(this UnitCard card)
    {
        if (PlayerUnits.Contains(card))
        {
            PlayerUnits.Remove(card);
            PhotonNetwork.Destroy(card.gameObject);

            return;
        }

        if (EnemyUnits.Contains(card))
        {
            PhotonNetwork.Destroy(card.gameObject);
            EnemyUnits.Remove(card);
        }
    }

    /// <summary> 적 카드 스폰시 이벤트 </summary>
    public static void CardSpawnEvent(this Action<UnitCard> action)
    {
        action?.Invoke(EnemyUnits.Last());
    }

    public static void SerializeUnitCards(this PhotonStream stream)
    {
        // 적과 나는 반대로 받아와야 함
        // 적 플레이어는 내 카드가 적 카드이기 때문
        stream.Serialize(PlayerUnits, EnemyUnits);
        MyDebug.Log($"Player Unit Count : {PlayerUnits.Count}\n Enemy Unit Count : {EnemyUnits.Count}");
        stream.Serialize(EnemyUnits, PlayerUnits);
    }
}
