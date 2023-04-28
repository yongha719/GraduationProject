using Photon.Pun;
using static MyDebug;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    public string PlayerName;

    void Start()
    {
        Log("Test Log");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Log($"{nameof(GameManager)} {nameof(OnPhotonSerializeView)}");
        stream.SerializeUnitCards();
    }
}
