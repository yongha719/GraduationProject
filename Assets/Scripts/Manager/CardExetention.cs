using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardExetention
{
    private static List<UnitCard> PlayerUnits => CardManager.Instance.PlayerUnits;
    private static List<UnitCard> EnemyUnits => CardManager.Instance.EnemyUnits;

    /// <summary> 플레이어의 카드 리스트에 추가 </summary>
    public static void AddPlayerUnit(this UnitCard card)
    {
        //PlayerUnits.Add(card);
    }

    /// <summary> 적의 카드 리스트에 추가 </summary>
    public static void AddEnemyUnit(this UnitCard card)
    {
        EnemyUnits.Add(card);
    }

    /// <returns> 플레이어의 카드 리스트를 반환 </returns>
    public static List<UnitCard> GetPlayerUnitCards(this UnitCard card)
    {
        return PlayerUnits;
    }

    /// <summary> Enemy의 Unit Card들을 반환 </summary>
    public static List<UnitCard> GetEnemyUnitCards(this UnitCard card)
    {
        return EnemyUnits;
    }

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
        // 적 플레이어는 내 카드가 적 카드이기 때문에 적과 나는 반대로 받아와야 함
        // List로 넘기고 싶었지만 직렬화때문에 안되는 것 같음..

        // TOOD : List로 넘기게 만들기
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(PlayerUnits.Count);
        //    foreach (UnitCard card in PlayerUnits)
        //        stream.SendNext(card);

        //    stream.SendNext(EnemyUnits.Count);
        //    foreach (UnitCard card in EnemyUnits)
        //        stream.SendNext(card);
        //}
        //else
        //{
        //    int count = (int)stream.ReceiveNext();
        //    EnemyUnits.Clear();

        //    for (int i = 0; i < count; i++)
        //        EnemyUnits.Add((UnitCard)stream.ReceiveNext());

        //    count = (int)stream.ReceiveNext();
        //    PlayerUnits.Clear();

        //    for (int i = 0; i < count; i++)
        //        PlayerUnits.Add((UnitCard)stream.ReceiveNext());
        //}
    }
}
