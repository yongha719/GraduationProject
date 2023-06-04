using Photon.Pun;
using System;
using UnityEngine;


/// <summary> 인게임 유닛 카드 </summary>
[Serializable]
public class UnitCard : Card, IPunObservable
{
    private int hp;

    public int Hp
    {
        get => hp;

        set => photonView.RPC(nameof(SetCardHpRPC), RpcTarget.AllBuffered, value);
    }

    public override CardState CardState
    {
        get => cardState;

        set => photonView.RPC(nameof(SetCardStateRPC), RpcTarget.AllBuffered, value);
    }

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];

    protected override void Awake()
    {
        base.Awake();

        Hp = CardData.Hp;
    }

    protected override void Start()
    {
        base.Start();

        if (IsEnemy)
        {
            CardManager.Instance.EnemyUnits.Add(this);
        }
        else
        {
            CardManager.Instance.PlayerUnits.Add(this);
        }
    }

    protected override void Attack()
    {
        if (CanAttack)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
            }

            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

            foreach (RaycastHit2D hit in raycastHits)
            {
                if (hit == null || hit.collider == null ||
                    hit.collider.TryGetComponent(out UnitCard enemyCard) == false ||
                    enemyCard.CanAttackThisCard() == false)
                    continue;

                enemyCard.Hit(damage: CardData.Damage);
            }
        }
    }

    [PunRPC]
    private void SetCardHpRPC(int value)
    {
        if (value > CardData.Hp)
            return;

        if (value <= 0)
        {
            hp = 0;

            if (IsEnemy)
                this.RemoveEnemyUnit();
            else
                this.RemovePlayerUnit();
        }
        
        hp = value;

        hpText.text = hp.ToString();
    }

    [PunRPC]
    private void SetCardStateRPC(CardState value)
    {
        cardState = value;

        switch (value)
        {
            case CardState.Deck:
                rect.localScale = Vector3.one;
                lineRenderer.positionCount = 0;
                break;
            case CardState.ExpansionDeck:
                rect.localScale = Vector3.one * 1.5f;
                // 덱에 있는 카드를 눌렀을 때 커지는 모션
                break;
            case CardState.Field:
                rect.localScale = Vector3.one * 0.6f;
                lineRenderer.positionCount = 2;
                break;
        }
    }

    public void Hit(int damage)
    {
        Hp -= damage;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }


    /// <summary> 이 카드를 공격할 수 있는지 확인 </summary>
    /// 이 메서드는 플레이어의 적 카드의 개체에서 호출됨
    public bool CanAttackThisCard()
    {
        // 이 카드가 적이고 필드에 도발 카드를 가지고 있는지 확인
        // 도발 카드가 없거나 이 카드가 도발 카드일 때 공격 가능
        if (IsEnemy && CardManager.Instance.HasEnemyTauntCard(this))
            return true;

        return false;
    }

    protected override void MoveCardFromDeckToField()
    {
        if (GameManager.Instance.IsTest)
            MoveCardFromDeckToFieldRPC();
        else
            photonView.RPC(nameof(MoveCardFromDeckToFieldRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void MoveCardFromDeckToFieldRPC()
    {
        PhotonView parentView = PhotonManager.GetFieldPhotonView(photonView.IsMine);
        CardState = CardState.Field;

        rect.SetParent(parentView.gameObject.transform);

        if (IsEnemy)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}