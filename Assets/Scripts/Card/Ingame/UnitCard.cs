using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class UnitCard : Card
{
    private int hp;

    [Tooltip("시봉봉의 억지")]
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

    public CardData CardData;

    protected override void Awake()
    {
        base.Awake();

        //Hp = CardData.Hp;
    }

    protected override void Start()
    {
        base.Start();

      
      
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }
}
