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
    private CardDeckLayout enemyDeck;

    [SerializeField] private TurnState turnState;

    public TurnState TurnState
    {
        get { return turnState; }

        set { turnState = value; }
    }

    public bool MyTurn => TurnState == TurnState.PlayerTurn;

    public event Action<UnitCard> OnEnemySpawnAction;

    public event Action OnPlayerTurnAction = () => { };

    public event Action OnTurnChangeAction = () => { };

    public event Action FirstTurnAction = () => { };

    [SerializeField] private TextMeshProUGUI testTurnStateText;

    [SerializeField] private Button turnChangeButton;

    [SerializeField] private Sprite turnFinishSprite;
    [SerializeField] private Sprite enemyTurnSprite;

    private static int playerTurnCount;
    private static int enemyTurnCount;

    private const string MY_TURN_CHANGE_ACTION_PATH = "Effect/MyTurnProduction";
    private GameObject myTurnChangeAction;

    private IEnumerator Start()
    {
        playerDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.PlayerDeck).GetComponent<CardDeckLayout>();
        enemyDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.EnemyDeck).GetComponent<CardDeckLayout>();

        turnChangeButton.onClick.AddListener(TurnChange);

        myTurnChangeAction = Resources.Load<GameObject>(MY_TURN_CHANGE_ACTION_PATH);

        yield return WaitTurn(2);

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
        turnChangeButton.image.sprite = MyTurn ? turnFinishSprite : enemyTurnSprite;

        testTurnStateText.text = TurnState.ToString();

        FirstTurnAction();
        
        playerDeck.CardDraw(3);

        if (MyTurn)
            playerTurnCount++;
        else
            enemyTurnCount++;
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

        if (MyTurn)
        {
            Instantiate(myTurnChangeAction, Vector3.zero, Quaternion.identity);
            OnPlayerTurnAction();
            playerTurnCount++;
        }
        else
            enemyTurnCount++;

        OnTurnChangeAction();

        yield return null;

        TurnBegin();
    }

    /// <summary> 턴이 끝났을때 </summary>
    public void TurnFinished()
    {
        TurnState = MyTurn ? TurnState.EnemyTurn : TurnState.PlayerTurn;

        turnChangeButton.interactable = MyTurn;
        turnChangeButton.image.sprite = MyTurn ? turnFinishSprite : enemyTurnSprite;
    }

    /// <summary> 턴이 시작했을 때 </summary>
    public void TurnBegin()
    {
        CardManager.Instance.HandleCards(MyTurn);

        if (playerTurnCount != 1 && MyTurn)
            CardManager.Instance.CardDraw();
    }

    public void ExecuteAfterTurn(int turnCount, Action call)
    {
        StartCoroutine(ExecuteAfterTurnCoroutine(turnCount, call));
    }

    public void ExecuteAfterTurn(int turnCount, Action beforeTurnCall, Action afterTurnCall)
    {
        StartCoroutine(ExecuteAfterTurnCoroutine(turnCount, beforeTurnCall, afterTurnCall));
    }

    private IEnumerator ExecuteAfterTurnCoroutine(int turnCount, Action beforeTurnCall, Action afterTurnCall)
    {
        beforeTurnCall();

        yield return WaitTurn(turnCount);

        afterTurnCall();
    }

    private IEnumerator ExecuteAfterTurnCoroutine(int turnCount, Action call)
    {
        yield return WaitTurn(turnCount);

        call();
    }

    /// <summary>
    /// N턴을 기다림
    /// </summary>
    public static CustomYieldInstruction WaitTurn(int turnCount)
    {
        var playerTurn = Instance.MyTurn;

        var curTurnCnt = playerTurn ? playerTurnCount : enemyTurnCount;

        return new WaitUntil(() =>
            (playerTurn ? playerTurnCount : enemyTurnCount) - curTurnCnt == turnCount);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}