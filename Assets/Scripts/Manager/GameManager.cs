using Photon.Pun;
using static MyDebug;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    public string PlayerName;

    void Start()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SerializeUnitCards();
    }
}
