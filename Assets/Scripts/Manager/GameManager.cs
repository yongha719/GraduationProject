using Photon.Pun;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    public string PlayerName;

    void Start()
    {
        print("Debug Test");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
