using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardManager
{
    private static List<UnitCard> PlayerUnits = new List<UnitCard>(10);
    private static List<UnitCard> EnemyUnits = new List<UnitCard>(10);

    private static Action enemySpawnEvent;
    public static Action EnemySpawnEvent
    {
        get => enemySpawnEvent;

        set
        {
            enemySpawnEvent = value;
        }
    }

    public static string Name;

    /// <summary> Player Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddPlayerUnit(this UnitCard card)
    {
        PlayerUnits.Add(card);
    }

    /// <summary> Enemy Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddEnemyUnit(this UnitCard card)
    {
        EnemyUnits.Add(card);

        if (enemySpawnEvent != null)
        {
            enemySpawnEvent();
        }
    }

    public static List<UnitCard> GetPlayerUnitCards(this UnitCard card) => PlayerUnits;
    public static List<UnitCard> GetEnemyUnitCards(this UnitCard card) => EnemyUnits;

    public static void RemovePlayerUnit(UnitCard card)
    {
        PlayerUnits.Remove(card);
    }


    /// <summary>  </summary>
    public static void RemoveEnemyUnit(UnitCard card)
    {
        EnemyUnits.Remove(card);
    }

    /// <summary> 적 소환할 때 이벤트 추가 </summary>
    public static void AddEnemySpawnEvent(System.Action call) => enemySpawnEvent += call;
}
