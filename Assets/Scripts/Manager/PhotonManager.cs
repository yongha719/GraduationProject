using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;


// PhotonNetwork Reference
// https://doc-api.photonengine.com/ko-kr/pun/current/class_photon_network.html

// MonoBehaviourPunCallbacks Reference
// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html


// 플레이어 관련 ViewId는 100부터 시작
// 적 관련 ViewId는 200부터 시작

/// <summary> 내 PhotonView들을 ViewId로 값으로 Type을 만들었음 </summary>
public enum PhotonViewType
{
    PlayerDeck = 100,
    PlayerField = 101,

    EnemyDeck = 200,
    EnemyField = 201,
}


public class PhotonManager : SingletonPunCallbacks<PhotonManager>
{
    private TypedLobby TestLobby = new TypedLobby("TestLobby", LobbyType.Default);
    private List<RoomInfo> roomInfos = new List<RoomInfo>();

    private Photon.Realtime.Player Player;
    private Photon.Realtime.Player EnemyPlayer;

    private Room room;

    private static Dictionary<PhotonViewType, PhotonView> PhotonViews = new Dictionary<PhotonViewType, PhotonView>();

    /// <summary> Type으로 PhotonView를 가져옴 </summary>
    public static PhotonView GetPhotonViewByType(PhotonViewType photonViewType) => PhotonViews[photonViewType];

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        foreach (PhotonViewType photonView in Enum.GetValues(typeof(PhotonViewType)))
        {
            PhotonViews.Add(photonView, PhotonView.Find((int)photonView));
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
    }

    public override void OnConnected()
    {
        print("On Connected");

        Player = PhotonNetwork.LocalPlayer;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TestLobby);
    }

    public override void OnJoinedLobby()
    {
        print("Join Lobby");

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("TestRoom", options, TestLobby);
    }


    public override void OnJoinedRoom()
    {
        print("Join Room");

        room = PhotonNetwork.CurrentRoom;
    }

    /// <summary> 방에 다른 플레이어가 들어왔을 때 </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer);

        TurnManager.Instance.FirstTurn();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print(nameof(OnRoomListUpdate));
        UpdateRoomList(roomList);
    }

    private void UpdateRoomList(List<RoomInfo> roomlist)
    {
        foreach (RoomInfo roominfo in roomlist)
        {
            if (roominfo.RemovedFromList)
            {
                roomInfos.Remove(roominfo);
            }
            else
            {
                roomInfos.Add(roominfo);
                print(roominfo.Name);
            }

        }
    }
}
