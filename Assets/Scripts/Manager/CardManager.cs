using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : SingletonPunCallbacks<CardManager>, IPunObservable
{
    // 필드에 낼 수 있는 카드가 최대 10개임
    public List<UnitCard> PlayerUnits = new List<UnitCard>(10);
    public List<UnitCard> EnemyUnits = new List<UnitCard>(10);

    // 포톤은 오브젝트를 리소스 폴더에서 가져와서 오브젝트의 이름으로 가져오기 위해 string으로 함
    private List<string> myDeck = new List<string>();

    /// <summary> - 내 덱 </summary>
    public List<string> MyDeck
    {
        get => myDeck;

        set
        {
            myDeck = value;
        }
    }

    /// <summary>
    /// 적이 도발 카드를 가지고 있는지 확인함 <br></br>
    /// 적이 도발 카드가 있다면 도발카드만 공격해야 하기 때문이다 <br></br>
    /// 만약 인자로 받은 카드가 도발 속성을 가지고 있으면 false 반환
    /// </summary>
    public bool EnemyHasTauntCard(UnitCard card)
    {
        if (card.CardData.CardAttributeType == CardAttributeType.Taunt)
            return false;

        return EnemyUnits.Select(card => card.CardData.CardAttributeType == CardAttributeType.Taunt) != null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 내가 보낼 때는 플레이어 카드를 보내고
        // 내가 보내는게 아닐 때는 적 카드를 받아옴

        //if (stream.IsWriting)
        //{
        //    stream.SendNext(PlayerUnits.Count);

        //    foreach (UnitCard card in PlayerUnits)
        //        stream.SendNext(card);
        //}
        //else
        //{
        //    int count = (int)stream.ReceiveNext();
        //    EnemyUnits.Clear();

        //    for (int i = 0; i < count; i++)
        //        EnemyUnits.Add((UnitCard)stream.ReceiveNext());
        //}

        //Serialize(stream, PlayerUnits, EnemyUnits);
        //Serialize(stream, EnemyUnits, PlayerUnits);
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
