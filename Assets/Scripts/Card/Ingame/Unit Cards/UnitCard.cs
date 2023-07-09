using Photon.Pun;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary> 인게임 유닛 카드의 정보파트 </summary>
[Serializable]
public class UnitCard : Card, IUnitCardSubject
{
    public UnitCardData CardData;

    [SerializeField] private int hp = 1;

    public virtual int Hp
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
                CardManager.Instance.AddUnitCard(this);
        }
    }

    public bool CanAttack =>
        IsEnemy == false && CardState == CardState.Field && TurnManager.Instance.MyTurn && isAttackableTurn;

    [SerializeField] protected bool isAttackableTurn = false;

    public bool HasSpecialAbility => CardData.UnitCardSpecialAbilityType != UnitCardSpecialAbilityType.None;

    public int Damage => CardData.Damage;

    public override int Cost => CardData.Cost;

    [Tooltip("저체온증 상태인지 체크")] public bool isHypothermic;

    [Tooltip("공격 턴 기다리는거 캐싱")] protected Action enableAttackCall;

    public bool IsHacked
    {
        set => isAttackableTurn = !value;
    }

    protected event Action OnFieldChangeAction = (() => { });

    [Header("Effect")] 
    
    
    private const string DAMAGE_EFFECT_PATH = "Effect/DamageEffect";
    
    [SerializeField] private DamageTextProduction damageEffect;

    
    private const string ILLUST_APPEAR_EFFECT_PATH = "Effect/DamageEffect";
    [SerializeField] private CharacterProduction illustAppearEffect;


    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    private BoxCollider2D boxCollider;

    protected override void Awake()
    {
        base.Awake();

        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        CardManager.Instance.TryGetCardData(name.Split("_")[0], ref CardData);
    }

    protected override void Start()
    {
        base.Start();

        damageEffect = Resources.Load<GameObject>(DAMAGE_EFFECT_PATH).GetComponent<DamageTextProduction>();
        illustAppearEffect = Resources.Load<GameObject>(ILLUST_APPEAR_EFFECT_PATH).GetComponent<CharacterProduction>();

        hp = CardData.Hp;

        boxCollider = GetComponent<BoxCollider2D>();

        enableAttackCall = () =>
            TurnManager.Instance.ExecuteAfterTurn(1,
                beforeTurnCall: () =>
                {
                    isAttackableTurn = false;
                    print($"Can't Attack :  {isAttackableTurn}");
                },
                afterTurnCall: () =>
                {
                    isAttackableTurn = true;
                    print($"Can Attack : {isAttackableTurn}");
                });

        if (CardData.UnitCardSpecialAbilityType == UnitCardSpecialAbilityType.Charge)
            isAttackableTurn = true;
    }

    private void Update()
    {
        if (CardState == CardState.Field && IsEnemy)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
    }

    [PunRPC]
    protected void SetCardHpRPC(int value)
    {
        if (GameManager.Instance.IsPlayerInvincibility)
            return;

        if (value > CardData.Hp)
            return;

        hp = value;

        if (value <= 0)
        {
            hp = 0;

            CardManager.Instance.RemoveUnitCard(this);
        }

        OnSetHpChange(hp);
    }

    [PunRPC]
    protected void SetCardStateRPC(CardState value)
    {
        cardState = value;

        switch (value)
        {
            case CardState.Deck:
                rect.localRotation = Quaternion.identity;
                rect.localScale = Vector3.one;
                if (IsEnemy == false)
                {
                    boxCollider.size *= 0.6667f;
                }

                break;
            case CardState.ExpansionDeck:
                if (IsEnemy == false)
                {
                    rect.localScale *= 1.5f;
                    boxCollider.size *= 1.5f;
                }

                // 덱에 있는 카드를 눌렀을 때 커지는 모션
                break;
            case CardState.Field:
                cardDragAndDrop.ShadowEnable = false;

                OnFieldChangeAction();
                cardInfo.OnFieldStateChange();

                TurnManager.Instance.ExecuteAfterTurn(1, call: () =>
                {
                    isAttackableTurn = true;
                    print($"{name} : now can attack  [{isAttackableTurn}]");
                });

                rect.anchoredPosition3D += Vector3.forward;
                rect.localScale = Vector3.one * 0.6f;
                break;
        }
    }


    /// <summary> 이 카드를 공격할 수 있는지 확인 </summary>
    /// 이 메서드는 플레이어의 적 카드의 개체에서 호출됨
    public bool CanAttackThisCard(UnitCard card)
    {
        if (IsEnemy == false) return false;

        // 이 카드가 적이고 필드에 도발 카드를 가지고 있는지 확인
        // 도발 카드가 없거나 이 카드가 도발 카드일 때 공격 가능
        if (CardManager.Instance.HasEnemyTauntCard)
            return card.CardData.UnitCardSpecialAbilityType == UnitCardSpecialAbilityType.Taunt;

        return true;
    }

    #region UnitCard Virtuals

    protected virtual void BasicAttack(UnitCard enemyCard)
    {
        enemyCard.Hit(Damage, Hit);
        print("Attack");
    }

    protected virtual void SpecialAbility()
    {
    }

    #endregion

    #region IUnitCardSubject override

    public int eventCountAfterNTurns { get; set; }

    public void HealCard(int healAmount)
    {
        Hp += healAmount;
    }

    public virtual void Hit(int damage)
    {
        Hp -= damage;
    }

    public virtual void Hit(int damage, Action<int> hitAction)
    {
        hitAction(Damage);

        Hit(damage);
    }

    public virtual void HandleTurn()
    {
        isAttackableTurn = true;
    }

    #endregion

    #region Card Overrides

    protected override void OnEndDrag()
    {
        if (CardState == CardState.Field && CanAttack == false)
            return;

        Attack();
    }

    protected override void OnDrop()
    {
        if (CardState == CardState.Field)
        {
            if (CanAttack == false)
                return;

            Attack();
        }

        if (cardState != CardState.Field && CanvasUtility.IsDropMyField())
            MoveCardFromDeckToField();
    }

    protected override void Attack()
    {
        if (CanAttack == false) return;

        print("Attack");

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is null ||
                hit.collider.TryGetComponent(out UnitCard enemyCard) == false ||
                enemyCard.CanAttackThisCard(enemyCard) == false)
                continue;

            BasicAttack(enemyCard);
            enableAttackCall();
        }
    }

    protected virtual void MoveCardFromDeckToField()
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

        cardDragAndDrop.ShadowEnable = false;

        rect.SetParent(parentView.transform);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    #endregion
}