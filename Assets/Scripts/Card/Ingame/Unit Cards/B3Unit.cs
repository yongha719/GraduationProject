using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B3Unit : UnitCard
{
    [SerializeField] private Vector3 dropPos;
    [SerializeField] private Vector3 startPos;

    private Vector2 centerPos = new Vector2(960, 0);

    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Instantiate(illustAppearEffect).GetComponent<CharacterProduction>().characterType = ECharacterType.Yuki;
    }

    protected override void Start()
    {
        base.Start();

        cardDragAndDrop.OnDrop += () => dropPos = rect.anchoredPosition;
    }

    protected override void BasicAttack(UnitCard enemyCard)
    {
        enemyCard.Hit(Damage);

        if (enemyCard.gameObject.activeSelf == false)
        {
            Hit(enemyCard.Damage);
            return;
        }

        startPos = cardDragAndDrop.OriginPos;

        // 여기에 두번 공격하는거
        StartCoroutine(EAttackAction(enemyCard));
    }

    private IEnumerator EAttackAction(UnitCard enemyCard)
    {
        Vector3 enemyPos = new Vector2(1920 - enemyCard.rect.anchoredPosition.x, 155);
        print($"enemy pos : {enemyPos}");

        Vector2 localPosition;

        // 밑에서 공격했을 때 enemy가 죽어서 null이 되면 참조를 못하기 때문에 캐싱해줌
        var enemyDamage = enemyCard.Damage;

        // 중앙으로 갔다가
        yield return EMoveToTarget(startPos, 0.4f);
        print("원래 위치");

        // 상대한테 공격
        yield return EMoveToTarget(enemyPos, 0.5f);
        print("공격");

        enemyCard.Hit(Damage);

        // 원래 위치로
        yield return EMoveToTarget(startPos, 0.5f);
        print("원래 위치");

        yield return new WaitForSeconds(0.4f);
        print("맞음");

        Hit(enemyDamage);
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