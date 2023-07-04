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

    [Tooltip("내 코스트"), SerializeField]
    private uint cost = 3;

    public uint Cost
    {
        get => cost;
        
        set
        {
            if(value > MaxCost)
            {
                cost = MaxCost;
                return;
            }
            
            cost = value;
        }
    }


    [Tooltip("내 최대 코스트"), SerializeField]
    private uint maxCost = 3;

    public uint MaxCost
    {
        get => maxCost;

        set
        {
            if(value > 10)
                return;

            maxCost = value;
        }
    }

    private GameObject LogCanvas;

    protected override void Awake()
    {
        base.Awake();

        LogCanvas = FindObjectOfType<LogManager>().gameObject;
    }

    private void Start()
    {
        TurnManager.Instance.OnPlayerTurnAction += () =>
        {
            IncreaseCost();
            print("Gm");
        };
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F11))
            LogCanvas?.SetActive(true);
    }

    private void IncreaseCost()
    {
        MaxCost++;
        Cost = MaxCost;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
