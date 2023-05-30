using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class CardExetention
{
    private static List<UnitCard> PlayerUnits;
    private static List<UnitCard> EnemyUnits;

    // Awake 다음에 호출됨
    // 대신 싱글톤 개체를 쓸 땐
    // 싱글턴 개체의 Script Excution Order를
    // Defalut Time 이전으로 바꿔줘야 함
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        PlayerUnits = CardManager.Instance.PlayerUnits;
        EnemyUnits = CardManager.Instance.EnemyUnits;
    }

    /// <summary> 플레이어의 카드 리스트에 추가 </summary>
    public static void AddPlayerUnit(this UnitCard card)
    {
        PlayerUnits.Add(card);
    }

    /// <summary> 적의 카드 리스트에 추가 </summary>
    public static void AddEnemyUnit(this UnitCard card)
    {
        EnemyUnits.Add(card);
    }
        
    /// <summary> 플레이어의 카드 리스트를 반환 </summary>
    public static List<UnitCard> GetPlayerUnitCards(this UnitCard card)
    {
        return PlayerUnits;
    }

    /// <summary> Enemy의 Unit Card들을 반환 </summary>
    public static List<UnitCard> GetEnemyUnitCards(this UnitCard card)
    {
        return EnemyUnits;
    }
    
    /// <summary> 플레이어의 카드 리스트에서 인자로 넘긴 카드를 제거 </summary>
    public static void RemovePlayerUnit(this UnitCard card)
    {
        int index = PlayerUnits.IndexOf(card);

        if (index >= 0)
        {
            PlayerUnits.RemoveAt(index);
            PhotonNetwork.Destroy(card.gameObject);
        }
    }

    /// <summary> 적의 카드 리스트에서 인자로 넘긴 카드를 제거 </summary>
    public static void RemoveEnemyUnit(this UnitCard card)
    {
        int index = EnemyUnits.IndexOf(card);
        if (index >= 0)
        {
            EnemyUnits.RemoveAt(index);
            PhotonNetwork.Destroy(card.gameObject);
        }
    }

    /// <summary> 적 카드 스폰시 이벤트 </summary>
    public static void CardSpawnEvent(this Action<UnitCard> action)
    {
        action?.Invoke(EnemyUnits.Last());
    }
}
