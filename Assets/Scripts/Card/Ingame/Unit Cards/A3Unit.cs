using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3Unit : UnitCard
{
    private const int HEAL_AMOUNT = 2;

    public override void HandleTurn()
    {
        CardManager.Instance.HealCards(HEAL_AMOUNT);

        // 아군은 2만큼 힐되고 자신은 1만큼 회복이라 1 빼줌
        Hp--;
    }
}
