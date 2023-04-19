using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// PhotonNetwork Reference
// https://doc-api.photonengine.com/ko-kr/pun/current/class_photon_network.html

// MonoBehaviourPunCallbacks Reference
// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }

    private TypedLobby TestLobby = new TypedLobby("TestLobby", LobbyType.Default);
    private List<RoomInfo> roomInfos = new List<RoomInfo>();


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(TestLobby);
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print(nameof(OnRoomListUpdate));
        UpdateRoomList(roomList);

    }

    public override void OnJoinedLobby()
    {
        print("Join Lobby");
    }

    public override void OnConnected()
    {
        print("On Connected");
    }

    //
    public override void OnConnectedToMaster()
    {
        print("안녕 내 이름은 김시원");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("안녕", options, TestLobby);
    }
}
