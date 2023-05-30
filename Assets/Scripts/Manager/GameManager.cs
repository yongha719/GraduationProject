using AYellowpaper.SerializedCollections;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    [Tooltip("플레이어 이름")]
    public string PlayerName;

    [Tooltip("테스트")]
    public bool IsTest;

    [Tooltip("플레이어 무적")]
    public bool IsPlayerInvincibility;

    [SerializeField]
    [Tooltip("카드 오브젝트들"), SerializedDictionary("Card Rating", "Card Prefab")]
    private SerializedDictionary<string, GameObject> CardPrefabs = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
