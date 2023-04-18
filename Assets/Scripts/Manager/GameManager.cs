using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerTurn;

    void Start()
    {
        PhotonManager.Instance.JoinLobby();
    }
}
