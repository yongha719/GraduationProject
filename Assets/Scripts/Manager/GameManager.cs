using AYellowpaper.SerializedCollections;
using Photon.Pun;
using UnityEngine;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    [Tooltip("플레이어 이름")]
    public string PlayerName;

    [Tooltip("테스트")]
    public bool IsTest;

    [Tooltip("플레이어 무적")]
    public bool IsPlayerInvincibility;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
