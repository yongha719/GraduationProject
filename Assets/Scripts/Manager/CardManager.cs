using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonPunCallbacks<CardManager>, IPunObservable
{
    // 필드에 낼 수 있는 카드가 최대 10개임
    public List<UnitCard> PlayerUnits = new List<UnitCard>(10);
    public List<UnitCard> EnemyUnits = new List<UnitCard>(10);

    // 포톤은 오브젝트를 리소스 폴더에서 가져와서 오브젝트 이름으로 저장했음
    private List<GameObject> myDeck = new List<GameObject>();
    /// <summary> 내 덱 </summary>
    public List<GameObject> MyDeck
    {
        get => myDeck;

        set
        {
            myDeck = value;
        }
    }

        

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 적 플레이어는 내 카드가 적 카드이기 때문에 적과 나는 반대로 받아와야 함
        Serialize(stream, PlayerUnits, EnemyUnits);
        Serialize(stream, EnemyUnits, PlayerUnits);
    }

    /// <summary> left를 보내고 right로 받음</summary>
    private void Serialize(PhotonStream stream, List<UnitCard> left, List<UnitCard> right)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(left.Count);

            foreach (UnitCard card in left)
                stream.SendNext(card);
        }
        else
        {
            int count = (int)stream.ReceiveNext();
            right.Clear();

            for (int i = 0; i < count; i++)
                right.Add((UnitCard)stream.ReceiveNext());
        }
    }
}
