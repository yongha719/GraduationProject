using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;




public class MasicCard : Card, IMasicCardSubject
{
    public virtual MasicAbilityTarget AbilityTarget { get; protected set; }

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    private bool CheckAbilityTargetConditionsAndExecuteAttack(Collider2D collider)
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
                GameManager.Instance.CheckCardCostAvailability(2, out Action costDecrease) &&
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