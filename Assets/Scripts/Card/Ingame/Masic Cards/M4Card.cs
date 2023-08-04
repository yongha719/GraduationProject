using System;
using System.Collections.Generic;
using System.Collections.Specialized;

/// <summary>
/// 상대유닛 4마리를 1턴동안 무력화(해킹)하는 카드
/// </summary>
public sealed class M4Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        var enemyCards = CardManager.Instance.PlayerUnitCards;

        var randomIndex = GetRandomIndex(4, enemyCards.Count);

        print(randomIndex);
        
        foreach (var index in randomIndex)
        {
            enemyCards[index].IsHacked = true;

            TurnManager.Instance.ExecuteAfterTurn(1, () =>
            {
                print(enemyCards[index].ToString());
                enemyCards[index].IsHacked = false;
            });
        }
    }

    List<int> GetRandomIndex(int count, int maxIndex)
    {
        if (count > maxIndex)
            count = maxIndex;
        
        var random = new Random();
        var result = new List<int>();

        while (result.Count < count)
        {
            int randIndex = random.Next(0, maxIndex);

            if (result.Contains(randIndex) == false)
            {
                result.Add(randIndex);
            }
        }

        return result;
    }
}