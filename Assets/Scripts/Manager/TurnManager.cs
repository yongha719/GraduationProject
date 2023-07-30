using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// 이 스크립트가 하는 역할===
// 턴 전환 관리
// N턴 뒤에 실행할 함수

public enum TurnState
{
    PlayerTurn,
    EnemyTurn
}

public class TurnManager : SingletonPunCallbacks<TurnManager>, IPunObservable
{
    [SerializeField]
    private TurnState turnState;

    public TurnState TurnState
    {
        get { return turnState; }

        set { turnState = value; }
    }

    [Tooltip("처음 드로우될 카드 갯수"), SerializeField]
    private int drawCardCount;

    public bool MyTurn => TurnState == TurnState.PlayerTurn;

    #region Events

    public event Action<UnitCard> OnEnemySpawnAction;

    /// <summary> 플레이어 턴이 됐을 때 이벤트  </summary>
    public event Action OnPlayerTurnAction = () => { };

    public event Action OnTurnChangeAction = () => { };

    public event Action FirstTurnAction = () => { };

    #endregion

    // TODO : 다른 스크립트로 옮기기

    #region Turn Change Action

    [SerializeField]
    private Button turnChangeButton;

    [SerializeField]
    private Sprite turnFinishSprite;

    [SerializeField]
    private Sprite enemyTurnSprite;

    private const string MY_TURN_CHANGE_ACTION_PATH = "Effect/MyTurnProduction";
    private GameObject myTurnChangeEffect;

    #endregion

    private static int playerTurnCount;
    private static int enemyTurnCount;

    public bool ShouldSummonCopy;
    private string enemySpawnCardName;

    private void Start()
    {
        turnChangeButton.onClick.AddListener(TurnChange);

        myTurnChangeEffect = Resources.Load<GameObject>(MY_TURN_CHANGE_ACTION_PATH);
    }

    /// <summary> 처음 턴 시작시 호출 </summary>
    public void FirstTurn()
    {
        photonView.RPC(nameof(firstTurnRPC), RpcTarget.AllBuffered);

        SoundManager.Instance.PlayBackGroundSound("InGameSound");
    }

    [PunRPC]
    private void firstTurnRPC()
    {
        TurnState = PhotonNetwork.IsMasterClient ? TurnState.PlayerTurn : TurnState.EnemyTurn;

        turnChangeButton.interactable = MyTurn;
        turnChangeButton.image.sprite = MyTurn ? turnFinishSprite : enemyTurnSprite;

        FirstTurnAction();

        CardManager.Instance.CardDraw(drawCardCount);

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


        if (MyTurn)
        {
            Instantiate(myTurnChangeEffect, Vector3.zero, Quaternion.identity);
            SoundManager.Instance.PlayDialogue(PhotonNetwork.IsMasterClient);

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
        {
            enemySpawnCardName = CardManager.Instance.CardDraw().name;
        }
    }

    /// <summary>
    /// 적 카드 소환할 때 복사해야하는지 검사하고 소환함
    /// </summary>
    /// <returns></returns>
    public bool CanEnemyCardCopyDraw()
    {
        if (ShouldSummonCopy)
        {
            CardManager.Instance.CardDraw(enemySpawnCardName, isPlayeDraw: false);
            print("적 카드 소환함 : " + enemySpawnCardName);
            ShouldSummonCopy = false;
            return true;
        }

        print("적 카드 소환 못함 : " + enemySpawnCardName);
        return false;
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
        if (stream.IsWriting && MyTurn == false)
        {
            stream.SendNext(enemySpawnCardName);
        }
        else if (stream.IsReading)
        {
            enemySpawnCardName = (string)stream.PeekNext();
        }
    }
}