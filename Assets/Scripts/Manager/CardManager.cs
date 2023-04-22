using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardManager
{
    private static List<UnitCard> PlayerUnits = new List<UnitCard>(10);
    private static List<UnitCard> EnemyUnits = new List<UnitCard>(10);

    private static Action EnemySpawnEvent;

    /// <summary> Player Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddPlayerUnit(UnitCard card)
    {
        PlayerUnits.Add(card);
    }

    /// <summary> Enemy Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddEnemyUnit(UnitCard card)
    {
        EnemyUnits.Add(card);

        if (EnemySpawnEvent != null)
        {
            EnemySpawnEvent();
        }
    }
    
    
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
    public static void AddEnemySpawnEvent(System.Action call) => EnemySpawnEvent += call;
}
