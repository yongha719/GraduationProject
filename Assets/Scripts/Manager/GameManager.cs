using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>, IPunObservable
{
    public bool IsPlayerTurn => TurnManager.Instance.TurnState == TurnState.PlayerTurn;

    void Start()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SerializeUnitCards();
    }
}
