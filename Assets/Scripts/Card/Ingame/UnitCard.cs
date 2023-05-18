using Photon.Pun;
using System;
using Unity.VisualScripting;
using UnityEngine;


/// <summary> 인게임 유닛 카드 </summary>
[Serializable]
public class UnitCard : Card, IPunObservable
{
    private int hp;

    public int Hp
    {
        get
        {
            return hp;
        }

        set
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
        }
    }

    public override CardState CardState
    {
        get => cardState;

        set => photonView.RPC(nameof(SetCardStateRPC), RpcTarget.AllBuffered, value);
    }

    protected override void Awake()
    {
        base.Awake();

        Hp = CardData.Hp;
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Attack()
    {
        if (CanAttack)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
            }

            // 임시 공격 시스템
            var rayhits = Physics2D.RaycastAll(transform.position + Vector3.back, Vector3.forward, 10f);

            for (int i = 0; i < rayhits.Length; i++)
            {
                if (rayhits[i].collider.TryGetComponent(out UnitCard enemyCard) && enemyCard.CanAttackThisCard())
                {
                    enemyCard.Hit(Damage: CardData.Damage);
                }
            }
        }
    }

    [PunRPC]
    private void SetCardStateRPC(CardState value)
    {
        switch (value)
        {
            case CardState.Deck:
                rect.localScale = Vector3.one;
                lineRenderer.positionCount = 0;
                break;
            case CardState.ExpansionDeck:
                // 덱에 있는 카드를 눌렀을 때 커지는 모션
                break;
            case CardState.Field:
                rect.localScale = Vector3.one * 0.6f;
                lineRenderer.positionCount = 2;
                break;
        }

        cardState = value;
    }

    public void Hit(int Damage)
    {
        Hp -= Damage;
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    /// <summary> 이 카드를 공격할 수 있는지 확인 </summary>`
    public bool CanAttackThisCard()
    {
        // 이 카드가 적이고 필드에 도발 카드를 가지고 있는지 확인
        // 도발 카드가 없거나 이 카드가 도발 카드일 때 공격 가능
        if (IsEnemy && CardManager.Instance.HasEnemyTauntCardAndCardIsTaunt(this))
            return true;
        else
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
        PhotonView parentView = PhotonManager.GetPhotonViewByType(photonView.IsMine ? PhotonViewType.PlayerField : PhotonViewType.EnemyField);
        CardState = CardState.Field;

        rect.SetParent(parentView.gameObject.transform);

        if (IsEnemy)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
