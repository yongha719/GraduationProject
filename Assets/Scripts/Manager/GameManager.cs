using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerTurn = true;

    void Start()
    {
        PhotonManager.Instance.JoinLobby();


    }

     /// <summary>  </summary>
    public void TurnChange()
    {
        
    }
}
