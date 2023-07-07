using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B3Unit : UnitCard
{
    protected override void BasicAttack(UnitCard enemyCard)
    {
        base.BasicAttack(enemyCard);

        // 여기에 두번 공격하는거
        // enemyCard.
        
        RectTransform enemyRect = enemyCard.transform as RectTransform;
        print(enemyRect);

        base.BasicAttack(enemyCard);
    }

    private IEnumerator EAttackAction(Vector3 targetPosition)
    {
        Vector3 startPosition = rect.anchoredPosition;
        
        float startTime = Time.time;

        while (Time.time - startTime < 1)
        {
            float t = (Time.time - startTime) / 1;

            // 보간된 위치 값을 계산합니다.
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // 등속으로 이동합니다.
            transform.position = newPosition;

            yield return null;
        }

        // 목표 위치에 도착한 경우, 위치를 정확히 맞춰줍니다.
        transform.position = targetPosition;
    }
}