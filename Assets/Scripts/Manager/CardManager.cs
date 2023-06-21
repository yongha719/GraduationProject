using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AYellowpaper.SerializedCollections;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : SingletonPunCallbacks<CardManager>, IPunObservable
{
    // 필드에 낼 수 있는 카드가 최대 10이기 때문에 Capacity를 10으로 정해줌
    public List<IUnitCardSubject> PlayerUnitCards = new(10);
    public List<IUnitCardSubject> EnemyUnitCards = new(10);

    public Action CardDraw;
    public Action EnemyCardDraw;

    [Tooltip("카드 데이터들"), SerializedDictionary("Card Rating", "Card Data")]
    public SerializedDictionary<string, CardData> CardDatas = new(15);

    private const string INGAME_CARD_PATH = "Cards/In game Cards";

    // 포톤은 오브젝트를 리소스 폴더에서 가져와서 오브젝트의 이름으로 가져오기 위해 string으로 함
    [SerializeField] private List<string> myDeckNames = new(20);

    /// <summary> - 내 덱 </summary>
    public List<string> MyDeckNames
    {
        get => myDeckNames;

        set => myDeckNames = value;
    }

    public bool HasEnemyTauntCard => tauntCardCount != 0;
    
    [Tooltip("도발 카드 갯수")]
    private uint tauntCardCount = 0;

    async void Start()
    {
        CardDatas = await ResourceManager.Instance.AsyncRequestCardData();
    }


    /// <summary>
    /// 카드의 등급에 맞는 CardData를 반환<br></br>
    /// 없을 경우 null반환
    /// </summary>
    /// <param name="name">카드의 등급을 인자로 받음</param>
    public void TryGetCardData(string name, ref CardData cardData)
    {
        if (CardDatas.TryGetValue(name, out cardData) == false)
            Debug.Assert(false, $"카드 등급이 없음 \n 카드 등급 : {name}");

        cardData = cardData.Copy();
    }

    /// <summary>
    /// 카드 등급을 카드 이름으로 하고
    /// </summary>
    /// <returns></returns>
    public string GetRandomCardName()
    {
        int randomIndex = Random.Range(0, MyDeckNames.Count);
        string cardName = MyDeckNames[randomIndex];

        MyDeckNames.RemoveAt(randomIndex);

        return cardName;
    }

    public void AddUnitCard(IUnitCardSubject card, bool isEnemy)
    {
        if (isEnemy)
            EnemyUnitCards.Add(card);
        else
            PlayerUnitCards.Add(card);
    }

    public void RemoveUnitCard(IUnitCardSubject card, bool isEnemy)
    {
        if (isEnemy)
            EnemyUnitCards.Remove(card);
        else
            PlayerUnitCards.Remove(card);

        card.Destroy();
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
