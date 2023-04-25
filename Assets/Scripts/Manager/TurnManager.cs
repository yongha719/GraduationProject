using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using Unity.VisualScripting;

public class TurnManager : SingletonPunCallbacks<TurnManager>
{
    private CardDeckLayout playerDeck = PhotonManager.GetPhotonViewByType(PhotonViewType.PlayerDeck).GetComponent<CardDeckLayout>();

    private void Start()
    {

    }

    /// <summary> 플레이어의 턴이 끝났을 때 </summary>
    public void PlayerFinished(Photon.Realtime.Player player)
    {

    }

    /// <summary> 턴이 시작했을 때 </summary>
    public void TurnBegins()
    {
        if (CardManager.EnemySpawnEvent != null)
        {

        }
    }

    /// <summary> 턴이 끝났을때? </summary>
    public void TurnFinished()
    {

    }

    /// <summary> 턴의 시간이 끝났을 때 </summary>
    public void TurnChange()
    {
        StartCoroutine(ETurnChange());
    }

    private IEnumerator ETurnChange()
    {
        // 나중에 턴 전환시 액션 넣을 예정
        playerDeck.CardDraw();

        yield return null;
    }

}
