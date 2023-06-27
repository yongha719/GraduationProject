using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private TurnState turnState;

    public TurnState TurnState
    {
        get { return turnState; }

        set { turnState = value; }
    }

    public bool MyTurn => TurnState == TurnState.PlayerTurn;

    public event Action<UnitCard> OnEnemySpawnCallBack;

    private event Action<bool, int> turnChangeEvent = (myTurn, turnCnt) => { };

    // 같은 함수를 여러번 등록했을 때 이벤트의 제거 방식이
    // Stack처럼 작동한다해서 이렇게 해줌
    public event Action<bool, int> TurnChangeEvent
    {
        add => turnChangeEvents.Enqueue(value);

        remove => turnChangeEvents.Dequeue();
    }

    private Queue<Action<bool, int>> turnChangeEvents = new(10);

    [SerializeField] private TextMeshProUGUI testTurnStateText;

    [SerializeField] private Button turnChangeButton;

    [SerializeField] private static int playerTurnCount;
    [SerializeField] private static int enemyTurnCount;

    private IEnumerator Start()
    {
        playerDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.PlayerDeck).GetComponent<CardDeckLayout>();

        turnChangeButton.onClick.AddListener(TurnChange);

        yield return WaitTurn(2, MyTurn);

        print("2턴뒤임");
    }

    public void PlayerCardDraw()
    {
        if (MyTurn)
            playerDeck.CardDraw(isTest: true);
    }

    /// <summary> 처음 턴 시작시 호출 </summary>
    public void FirstTurn()
    {
        photonView.RPC(nameof(firstTurnRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void firstTurnRPC()
    {
        TurnState = PhotonNetwork.IsMasterClient ? TurnState.PlayerTurn : TurnState.EnemyTurn;

        turnChangeButton.interactable = MyTurn;
        testTurnStateText.text = TurnState.ToString();

        if (MyTurn)
            playerDeck.CardDraw();

        playerTurnCount++;
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

    /// <summary> 턴이 끝났을때 </summary>
    public void TurnFinished()
    {
        TurnState = MyTurn ? TurnState.EnemyTurn : TurnState.PlayerTurn;

        turnChangeButton.interactable = MyTurn;
    }

    /// <summary> 턴이 시작했을 때 </summary>
    public void TurnBegin()
    {

        if (MyTurn)
            playerTurnCount++;
        else
            enemyTurnCount++;

        CardManager.Instance.HandleCards(MyTurn);

        if (MyTurn)
            CardManager.Instance.CardDraw();
        // else
        //     OnEnemySpawnCallBack.CardSpawnEvent();
    }

    /// <summary>
    /// N턴을 기다림
    /// </summary>
    public static CustomYieldInstruction WaitTurn(int turnCount, bool playerTurn)
    {
        var curTurnCnt = playerTurn ? playerTurnCount : enemyTurnCount;

        return new WaitUntil(() =>
            (playerTurn ? playerTurnCount : enemyTurnCount) - curTurnCnt == turnCount);
    }

    public void ExecuteAfterTurn(int turnCount, bool playerTurn, Action call)
    {
        StartCoroutine(ExecuteAfterTurnCoroutine(turnCount, playerTurn, call));
    }

    private IEnumerator ExecuteAfterTurnCoroutine(int turnCount, bool playerTurn, Action call)
    {
        yield return WaitTurn(turnCount, playerTurn);

        call();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}