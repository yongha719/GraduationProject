using Photon.Pun;
using System;
using UnityEngine;

/// <summary> 인게임 유닛 카드의 정보파트 </summary>
[Serializable]
public class UnitCard : Card, IUnitCardSubject
{
    private int hp = 1;

    public int Hp
    {
        get => hp;

        set => photonView.RPC(nameof(SetCardHpRPC), RpcTarget.AllBuffered, value);
    }

    public event Action<int> OnSetHpChange = i => { };

    public override CardState CardState
    {
        get => cardState;

        set
        {
            photonView.RPC(nameof(SetCardStateRPC), RpcTarget.AllBuffered, value);

            if (cardState == CardState.Field)
                CardManager.Instance.AddUnitCard(this, IsEnemy);
        }
    }

    public bool HasSpecialAbility => CardData.CardSpecialAbilityType != CardSpecialAbilityType.None;

    public int Damage => CardData.Damage;

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    protected override void Start()
    {
        base.Start();

        hp = CardData.Hp;
        
        
    }

    [PunRPC]
    protected void SetCardHpRPC(int value)
    {
        if (value > CardData.Hp)
            return;

        if (value <= 0)
        {
            hp = 0;

            CardManager.Instance.RemoveUnitCard(this, IsEnemy);
        }

        hp = value;

        OnSetHpChange(hp);
    }

    [PunRPC]
    protected void SetCardStateRPC(CardState value)
    {
        cardState = value;

        switch (value)
        {
            case CardState.Deck:
                rect.localScale = Vector3.one;
                break;
            case CardState.ExpansionDeck:
                if (IsEnemy == false)
                {
                    rect.localScale = Vector3.one * 1.5f;
                }

                // 덱에 있는 카드를 눌렀을 때 커지는 모션
                break;
            case CardState.Field:
                cardInfo.OnFieldStateChange();

                rect.localScale = Vector3.one * 0.6f;

                if (IsEnemy)
                    rect.Rotate(0, 0, 180);
                break;
        }
    }

    protected override void Attack()
    {
        if (CanAttack == false) return;

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is null ||
                hit.collider.TryGetComponent(out UnitCard enemyCard) == false ||
                enemyCard.CanAttackThisCard() == false)
                continue;

            BasicAttack(enemyCard);
        }
    }

    protected virtual void BasicAttack(UnitCard card)
    {
        card.Hit(Damage, Hit);
    }

    protected virtual void SpecialAbility()
    {
        if (HasSpecialAbility == false) return;
    }

    public void Hit(int damage)
    {
        Hp -= damage;
    }
    
    public void Hit(int damage, Action<int> hitAction)
    {
        hitAction(Damage);
        Hp -= damage;
    }

    /// <summary> 이 카드를 공격할 수 있는지 확인 </summary>
    /// 이 메서드는 플레이어의 적 카드의 개체에서 호출됨
    public bool CanAttackThisCard()
    {
        if (IsEnemy == false) return false;

        // 이 카드가 적이고 필드에 도발 카드를 가지고 있는지 확인
        // 도발 카드가 없거나 이 카드가 도발 카드일 때 공격 가능
        if (CardManager.Instance.HasEnemyTauntCard)
            return CardData.CardSpecialAbilityType == CardSpecialAbilityType.Taunt;

        return true;
    }

    protected override void MoveCardFromDeckToField()
    {
        if (GameManager.Instance.IsTest)
            MoveCardFromDeckToFieldRPC();
        else
            photonView.RPC(nameof(MoveCardFromDeckToFieldRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    protected void MoveCardFromDeckToFieldRPC()
    {
        var parentView = PhotonManager.GetFieldPhotonView(photonView.IsMine);
        CardState = CardState.Field;

        rect.SetParent(parentView.transform);

        if (IsEnemy)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    public void HealCard(int healAmount)
    {
        Hp += healAmount;
    }
}