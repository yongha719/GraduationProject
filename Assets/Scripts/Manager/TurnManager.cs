using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    private Action<UnitCard> enemySpawnEvent;
    public Action<UnitCard> EnemySpawnEvent
    {
        get => enemySpawnEvent;

        set
        {
            enemySpawnEvent = value;
        }
    }


    [SerializeField]
    private TextMeshProUGUI TestTurnStateText;

    /// <summary> 적 소환할 때 이벤트 추가 </summary>
    public void AddEnemySpawnEvent(System.Action<UnitCard> call) => enemySpawnEvent += call;

    private void Start()
    {
        playerDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.PlayerDeck).GetComponent<CardDeckLayout>();
    }

    public void FirstTurn()
    {
        TurnState = PhotonNetwork.IsMasterClient ? TurnState.PlayerTurn : TurnState.EnemyTurn;

        if (MyTurn)
            playerDeck.CardDraw();
    }

    /// <summary> 턴의 시간이 끝났을 때 </summary>
    public void TurnChange()
    {
        photonView.RPC(nameof(TurnChangeRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void TurnChangeRPC() => StartCoroutine(ETurnChange());
    private IEnumerator ETurnChange()
    {
        TurnFinished();

        // 나중에 턴 전환시 액션 넣을 예정
        yield return null;

        TurnBegin();
    }

    /// <summary> 턴이 끝났을때? </summary>
    public void TurnFinished()
    {
        TurnState = (MyTurn) ? TurnState.EnemyTurn : TurnState.PlayerTurn;

        TestTurnStateText.text = TurnState.ToString();
    }

    /// <summary> 턴이 시작했을 때 </summary>
    public void TurnBegin()
    {
        if (MyTurn)
            playerDeck.CardDraw();
        else
        {
            EnemySpawnEvent.CardSpawnEvent();
        }
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
