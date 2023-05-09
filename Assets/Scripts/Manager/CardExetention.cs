using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CardExetention
{
    /// <summary> 플레이어의 카드 리스트에 추가 </summary>
    public static void AddPlayerUnit(this UnitCard card)
    {
        CardManager.Instance.PlayerUnits.Add(card);
    }

    /// <summary> 적의 카드 리스트에 추가 </summary>
    public static void AddEnemyUnit(this UnitCard card)
    {
        CardManager.Instance.EnemyUnits.Add(card);
    }

    /// <returns> 플레이어의 카드 리스트를 반환 </returns>
    public static List<UnitCard> GetPlayerUnitCards(this UnitCard card)
    {
        return CardManager.Instance.PlayerUnits;
    }

    /// <summary> Enemy의 Unit Card들을 반환 </summary>
    public static List<UnitCard> GetEnemyUnitCards(this UnitCard card)
    {
        return CardManager.Instance.EnemyUnits;
    }

    public static void RemoveUnit(this UnitCard card)
    {
        if (CardManager.Instance.PlayerUnits.Contains(card))
        {
            CardManager.Instance.PlayerUnits.Remove(card);
            PhotonNetwork.Destroy(card.gameObject);

            return;
        }

        if (CardManager.Instance.EnemyUnits.Contains(card))
        {
            PhotonNetwork.Destroy(card.gameObject);
            CardManager.Instance.EnemyUnits.Remove(card);
        }
    }


    /// <summary> 적 카드 스폰시 이벤트 </summary>
    public static void CardSpawnEvent(this Action<UnitCard> action)
    {
        action?.Invoke(CardManager.Instance.EnemyUnits.Last());
    }
}
