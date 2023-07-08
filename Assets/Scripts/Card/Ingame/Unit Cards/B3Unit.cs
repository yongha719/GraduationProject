using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B3Unit : UnitCard
{
    private Vector3 dropPos;
    private Vector3 startPos;

    protected override void Start()
    {
        base.Start();

        cardDragAndDrop.OnDrop += () => { dropPos = rect.anchoredPosition; };
    }

    protected override void BasicAttack(UnitCard enemyCard)
    {
        enemyCard.Hit(Damage);

        startPos = rect.anchoredPosition;
        print("Start pos : " + startPos);
        rect.anchoredPosition = dropPos;
        // 여기에 두번 공격하는거
        StartCoroutine(EAttackAction(enemyCard));
    }

    private IEnumerator EAttackAction(UnitCard enemyCard)
    {
        Vector3 enemyPos = new Vector2(-enemyCard.rect.anchoredPosition.x, 155);

        Vector2 localPosition;

        // 중앙으로 갔다가
        yield return EMoveToTarget(new Vector3(startPos.x, 0), 1f);

        // 상대한테 공격
        yield return EMoveToTarget(enemyPos, 0.5f);

        enemyCard.Hit(Damage);

        //다시 중앙으로 감
        yield return EMoveToTarget(new Vector3(startPos.x, 0), 0.5f);

        // 원래 위치로
        yield return EMoveToTarget(startPos, 0.5f);

        yield return new WaitForSeconds(0.4f);

        Hit(enemyCard.Damage);
    }

    private IEnumerator EMoveToTarget(Vector3 targetPos, float duration)
    {
        Vector3 startPosition = rect.anchoredPosition;

        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;

            // 보간된 위치 값을 계산합니다.
            Vector3 lerp = Vector3.Lerp(startPosition, targetPos, t);

            // 등속으로 이동합니다.
            rect.anchoredPosition = lerp;

            yield return null;
        }

        // 목표 위치에 도착한 경우, 위치를 정확히 맞춰줍니다.
        rect.anchoredPosition = targetPos;
    }
}