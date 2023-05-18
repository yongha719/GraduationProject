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
                //this.RemoveUnit();
            }

            hp = value;
        }
    }

    public override CardState CardState
    {
        get => cardState;

        set
        {
            photonView.RPC(nameof(SetCardStateRPC), RpcTarget.AllBuffered, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Hp = CardData.Hp;
    }

    protected override void Start()
    {
        base.Start();

        print(nameof(UnitCard));
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
    }
}
