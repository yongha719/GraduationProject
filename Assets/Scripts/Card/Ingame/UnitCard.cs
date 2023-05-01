using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;


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
                MyDebug.Log("피 없엉");
                this.RemoveUnit();
            }

            hp = value;
        }
    }

    public override CardState CardState
    {
        get
        {
            return cardState;
        }
        set
        {
            cardState = value;

            if (cardState == CardState.Deck)
            {
                rect.localScale = Vector3.one;
            }
            else if (cardState == CardState.Field)
            {

                rect.localScale = Vector3.one * 0.6f;
            }
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
        photonView.RPC(nameof(MoveCardFromDeckToFieldRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void MoveCardFromDeckToFieldRPC()
    {
        PhotonView parentView = PhotonManager.GetPhotonViewByType(photonView.IsMine ? PhotonViewType.PlayerField : PhotonViewType.EnemyField);

        rect.SetParent(parentView.gameObject.transform);
    }
}
