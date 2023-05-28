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

    [Tooltip("카드 데이터들"), SerializedDictionary("Card Rating", "Card Data")]
    public SerializedDictionary<string, CardData> CardDatas = new();

    [SerializeField]
    [Tooltip("카드 오브젝트들"), SerializedDictionary("Card Rating", "Card Prefab")]
    private SerializedDictionary<string, GameObject> CardPrefabs = new();

    protected override void Awake()
    {
        base.Awake();
    }

    async void Start()
    {
        CardDatas = await ResourceManager.Instance.AsyncRequestCardData() as SerializedDictionary<string, CardData>;
    }

    /// <summary>
    /// 카드의 등급에 맞는 CardData를 반환
    /// 없을 경우 null반환
    /// </summary>
    /// <param name="name">카드의 등급을 인자로 받음</param>
    public CardData GetCardData(string name)
    {
        if (CardDatas.ContainsKey(name))
            return CardDatas[name].Copy();

        return null;
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
