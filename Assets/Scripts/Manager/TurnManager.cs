using Photon.Pun;
using System;
using System.Collections;
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

    private Action<UnitCard> enemySpawnEvent;
    public Action<UnitCard> EnemySpawnEvent
    {
        get => enemySpawnEvent;

        set
        {
            enemySpawnEvent = value;
        }
    }

    /// <summary> 적 소환할 때 이벤트 추가 </summary>
    public void AddEnemySpawnEvent(System.Action<UnitCard> call) => enemySpawnEvent += call;

    private void Start()
    {
        playerDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.PlayerDeck).GetComponent<CardDeckLayout>();
    }

    public void FirstTurn()
    {
        TurnState = PhotonNetwork.IsMasterClient ? TurnState.PlayerTurn : TurnState.EnemyTurn;
        TurnBegin();
    }
    
    /// <summary> 턴의 시간이 끝났을 때 </summary>
    public void TurnChange()
    {
        StartCoroutine(ETurnChange());
    }

    private IEnumerator ETurnChange()
    {
        TurnFinished();

        // 나중에 턴 전환시 액션 넣을 예정
        yield return null;

        TurnBegin();
    }

    /// <summary> 턴이 시작했을 때 </summary>
    public void TurnBegin()
    {
        if (TurnState == TurnState.PlayerTurn)
            playerDeck.CardDraw();
        else
        {
            EnemySpawnEvent.CardSpawnEvent();
        }
    }

    /// <summary> 턴이 끝났을때? </summary>
    public void TurnFinished()
    {
        TurnState = (TurnState == TurnState.PlayerTurn) ? TurnState.EnemyTurn : TurnState.PlayerTurn;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (TurnState == TurnState.PlayerTurn)
                stream.SendNext(TurnState.EnemyTurn);
            else
                stream.SendNext(TurnState.PlayerTurn);
        }
        else
        {
            TurnState = (TurnState)stream.ReceiveNext();
        }
    }
}
