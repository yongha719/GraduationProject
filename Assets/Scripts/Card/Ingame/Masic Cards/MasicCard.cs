using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class MasicCard : Card, IMasicCardSubject
{
    [field: SerializeField]
    public MasicCardData MasicCardData;

    private uint cost => (uint)MasicCardData.Cost;

    public MasicAbilityTarget AbilityTarget => MasicCardData.MasicAbilityTarget;

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    protected override void Awake()
    {
        base.Awake();

        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        CardManager.Instance.TryGetCardData(name, out MasicCardData);
        print(cost);
    }

    protected virtual void Start()
    {
        base.Start();
    }


    protected override void DropField()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is not null &&
                GameManager.Instance.CheckCardCostAvailability(cost, out Action costDecrease) &&
                CheckAbilityTargetConditionsAndUseAbility(hit.collider))
            {
                costDecrease();
                print(cost);

                print("Use Ability");
                Destroy();
            }
        }
    }

    /// <summary>
    /// 들어온 콜라이더를 검사해서 마법 사용 타입에 따라 마법을 사용하고 값을 반환함
    /// </summary>
    private bool CheckAbilityTargetConditionsAndUseAbility(Collider2D collider)
    {
        if (CheckAbilityTargetConditions(out UnitCard card, collider))
        {
            if (CardManager.Instance.CanUseMasicCard == false)
                return false;

            Ability(card);
            return true;
        }

        return false;
    }

    private bool CheckAbilityTargetConditions<T>(out T t, Collider2D collider) where T : UnitCard
    {
        t = null;

        switch (AbilityTarget)
        {
            case MasicAbilityTarget.Field:
                if (collider.TryGetComponent(out CardFieldLayout layout))
                    return true;
                break;
                
            case MasicAbilityTarget.Ally:
                if (collider.TryGetComponent(out UnitCard card) && card.IsMine)
                {
                    t = (T)card;
                    
                    return true;
                }
                break;
                
            case MasicAbilityTarget.Enemy:
                if (collider.TryGetComponent(out UnitCard enemycard) &&
                    enemycard.CanAttackThisCard(enemycard))
                {
                    t = (T)enemycard;
                    
                    return true;
                }
                break;
                
        }

        return false;
    }

    protected override void OnEndDrag()
    {
        DropField();
    }

    public virtual void Ability(UnitCard card = null)
    {
    }
}