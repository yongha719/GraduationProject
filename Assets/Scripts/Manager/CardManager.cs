using Photon.Pun;
using System;
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

    void Start()
    {
        CardExetention.Init();
    }

    /// <summary>
    /// 적이 도발 카드를 가지고 있는지 확인함 <br></br>
    /// </summary>
    /// 이 카드는 플레이어 클라이언트 기준으로 적 카드이기 때문에 적 카드 리스트에서 확인함
    public bool CanAttackWhenTaunt(UnitCard card)
    {
        // 인자로 받은 카드가 Taunt 속성을 가지고 있으면 true 반환
        if (card.CardData.CardAttributeType == CardAttributeType.Taunt)
            return true;

        // 적 카드 중에 Taunt 속성이 있는지 확인
        bool hasEnemyTauntCard = EnemyUnits.Exists(enemyCard => enemyCard.CardData.CardAttributeType == CardAttributeType.Taunt);

        // 인자로 넘긴 카드가 Taunt 속성이 아니고, 적 카드 중에도 Taunt 속성이 없으면 true 반환
        if (card.CardData.CardAttributeType != CardAttributeType.Taunt && hasEnemyTauntCard == false)
            return true;

        // 인자로 넘긴 카드가 Taunt 속성이 아니고, 적 카드 중에 Taunt 속성이 있으면 false 반환
        return false;
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

            foreach (var card in left)
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
