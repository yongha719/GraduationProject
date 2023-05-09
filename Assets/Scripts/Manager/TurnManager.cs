using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn
}

public class TurnManager : SingletonPunCallbacks<TurnManager>, IPunObservable
{
    private CardDeckLayout playerDeck;

    [SerializeField]
    private TurnState turnState;
    public TurnState TurnState
    {
        get
        {
            return turnState;
        }

        set
        {
            turnState = value;
        }
    }

    public bool MyTurn => TurnState == TurnState.PlayerTurn;

    private Action<UnitCard> onEnemySpawnCallBack;
    public Action<UnitCard> OnEnemySpawnCallBack
    {
        get => onEnemySpawnCallBack;

        set
        {
            onEnemySpawnCallBack = value;
        }
    }


    [SerializeField]
    private TextMeshProUGUI testTurnStateText;

    [SerializeField]
    private Button turnChangeButton;

    private void Start()
    {
        playerDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.PlayerDeck).GetComponent<CardDeckLayout>();

        turnChangeButton.onClick.AddListener(() =>
        {
            if (MyTurn)
            {
                TurnChange();
            }
        });
    }

    /// <summary> 처음 턴 시작시 호출 </summary>
    public void FirstTurn()
    {
        photonView.RPC(nameof(FirstTurnRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void FirstTurnRPC()
    {
        TurnState = PhotonNetwork.IsMasterClient ? TurnState.PlayerTurn : TurnState.EnemyTurn;

        turnChangeButton.interactable = MyTurn;
        testTurnStateText.text = TurnState.ToString();

        if (MyTurn)
            playerDeck.CardDraw();
    }

    /// <summary> 턴의 시간이 끝났을 때 </summary>
    public void TurnChange()
    {
        photonView.RPC(nameof(TurnChangeRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void TurnChangeRPC()
    {
        StartCoroutine(ETurnChange());
    }

    private IEnumerator ETurnChange()
    {
        TurnFinished();

        // 나중에 턴 전환시 액션 넣을 예정
        testTurnStateText.text = TurnState.ToString();
        yield return null;

        TurnBegin();
    }

    /// <summary> 턴이 끝났을때? </summary>
    public void TurnFinished()
    {
        TurnState = (MyTurn) ? TurnState.EnemyTurn : TurnState.PlayerTurn;

        turnChangeButton.interactable = MyTurn;
    }

    /// <summary> 턴이 시작했을 때 </summary>
    public void TurnBegin()
    {
        if (MyTurn)
            playerDeck.CardDraw();
        else
            OnEnemySpawnCallBack.CardSpawnEvent();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MyTurn ? TurnState.EnemyTurn : TurnState.PlayerTurn);
        }
        else
        {
            TurnState = (TurnState)stream.ReceiveNext();
        }
    }
}
