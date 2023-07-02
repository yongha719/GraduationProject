using System;
using AYellowpaper.SerializedCollections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    [Tooltip("플레이어 이름")]
    public string PlayerName;

    [Tooltip("테스트")]
    public bool IsTest;

    [Tooltip("플레이어 무적")]
    public bool IsPlayerInvincibility;

    [Tooltip("내 코스트")] 
    public uint Cost;
    
    [Tooltip("내 최대 코스트"), SerializeField]
    private uint maxCost = 3;

    public uint MaxCost => maxCost;
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TurnManager.Instance.OnPlayerTurnAction += () =>
        {
            IncreaseCost();
            print("Gm");
        };
    }

    
    private void IncreaseCost()
    {
        Cost++;
        maxCost++;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
