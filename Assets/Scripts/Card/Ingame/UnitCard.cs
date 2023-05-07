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

        rect.localScale = Vector3.one * 0.6f;
        rect.SetParent(parentView.gameObject.transform);
    }
}
