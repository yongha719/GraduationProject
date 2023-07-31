using System;
using UnityEngine;


public class MasicCard : Card, IMasicCardSubject
{
    private const int COST = 2;

    [field: SerializeField]
    public MasicCardData MasicCardData;

    public MasicAbilityTarget AbilityTarget => MasicCardData.MasicAbilityTarget;

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    protected override void Awake()
    {
        base.Awake();
        
        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        CardManager.Instance.TryGetCardData(name, ref MasicCardData);
    }

    protected virtual void Start()
    {
        base.Start();
    }
    
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
            card.IsMine)
        {
            Ability(card);
            return true;
        }

        if (AbilityTarget == MasicAbilityTarget.Enemy && collider.TryGetComponent(out UnitCard enemycard) &&
            enemycard.CanAttackThisCard(enemycard))
        {
            Ability(enemycard);
            return true;
        }

        return false;
    }

    protected override void DropField()
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
                
                Destroy();
                print("Use Ability");
            }
        }
    }

    protected override void OnEndDrag()
    {
        DropField();
    }

    public virtual void Ability()
    {
    }

    public virtual void Ability(UnitCard card)
    {
    }
}