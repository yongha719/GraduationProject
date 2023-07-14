using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;




public class MasicCard : Card, IMasicCardSubject
{
    private const int COST = 2;

    public MasicCardData MasicCardData
    {
        get; private set;
    }

    public virtual MasicAbilityTarget AbilityTarget { get; }

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    /// <summary>
    /// 들어온 콜라이더를 검사해서 마법 사용 타입에 따라 마법을 사용하고 값을 반환함
    /// </summary>
    protected virtual bool CheckAbilityTargetConditionsAndExecuteAttack(Collider2D collider)
    {
        if (AbilityTarget == MasicAbilityTarget.Field && collider.TryGetComponent(out CardFieldLayout layout))
        {
            Ability();
            return true;
        }

        if (AbilityTarget == MasicAbilityTarget.Ally && collider.TryGetComponent(out UnitCard card) &&
            card.IsEnemy == false)
        {
            Ability(card);
            return true;
        }

        if (AbilityTarget == MasicAbilityTarget.Enemy && collider.TryGetComponent(out UnitCard enemycard) &&
            enemycard.IsEnemy)
        {
            Ability(enemycard);
            return true;
        }

        return false;
    }

    protected override void Attack()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is not null &&
                GameManager.Instance.CheckCardCostAvailability(COST, out Action costDecrease) &&
                CheckAbilityTargetConditionsAndExecuteAttack(hit.collider))
            {
                costDecrease();
                print("Use Ability");
            }
        }
    }

    protected override void OnEndDrag()
    {
        Attack();
    }

    public virtual void Ability()
    {
    }

    public virtual void Ability(UnitCard card)
    {
    }
}