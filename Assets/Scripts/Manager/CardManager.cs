using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardManager
{
    private static List<IUnitCard> PlayerUnits = new List<IUnitCard>(10);
    private static List<IUnitCard> EnemyUnits = new List<IUnitCard>(10);

    private static Action EnemySpawnEvent;

    /// <summary> Player Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddPlayerUnit(IUnitCard card)
    {
        PlayerUnits.Add(card);
    }

    /// <summary> Enemy Unit Card를 필드에 스폰했을 때 </summary>
    public static void AddEnemyUnit(IUnitCard card)
    {
        EnemyUnits.Add(card);

        if (EnemySpawnEvent != null)
        {
            EnemySpawnEvent();
        }
    }
    
    
    public static void RemovePlayerUnit(IUnitCard card)
    {
        PlayerUnits.Remove(card);
    }


    public static void RemoveEnemyUnit(IUnitCard card)
    {
        EnemyUnits.Remove(card);
    }

    /// <summary> 적 소환할 때 이벤트 추가 </summary>
    public static void AddSpawnEvent(System.Action call) => EnemySpawnEvent += call;

    public static void HealUnit(int amountofheal)
    {
        foreach (var unit in PlayerUnits)
        {
            unit.Heal(amountofheal);
        }
    }
}
