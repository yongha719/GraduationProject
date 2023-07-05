using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;


public enum MasicAbilityTarget
{
    Field,
    Ally,
    Enemy,
}

public class MasicCard : Card, IMasicCardSubject
{
    public virtual MasicAbilityTarget AbilityTarget { get; }

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
            if (hit.collider is not null && CheckAbilityTargetConditionsAndExecuteAttack(hit.collider))
                print("Use Ability");
        }
    }

    protected override void OnEndDrag()
    {
        if (GameManager.Instance.CheckCardCostAvailability())
        {
        }
    }

    public virtual void Ability()
    {
    }

    public virtual void Ability(UnitCard card)
    {
    }
}