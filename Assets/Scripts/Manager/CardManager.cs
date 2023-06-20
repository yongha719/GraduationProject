using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using AYellowpaper.SerializedCollections;


public class CardManager : SingletonPunCallbacks<CardManager>, IPunObservable
{
    // 필드에 낼 수 있는 카드가 최대 10이기 때문에 Capacity를 10으로 정해줌
    public List<UnitCard> PlayerUnitsCard = new(10);
    [FormerlySerializedAs("EnemyUnits")] public List<UnitCard> EnemyUnitCard = new(10);

    public Action CardDraw;
    public Action EnemyCardDraw;

    [Tooltip("카드 데이터들"), SerializedDictionary("Card Rating", "Card Data")]
    public SerializedDictionary<string, CardData> CardDatas = new();

    private const string INGAME_CARD_PATH = "Cards/In game Cards";

    // GameObject로 가져올 수 있게 커스텀해서 안 쓸듯
    // 일단 남겨둠
    // 포톤은 오브젝트를 리소스 폴더에서 가져와서 오브젝트의 이름으로 가져오기 위해 string으로 함
    [SerializeField] private List<string> myDeckNames = new List<string>();

    /// <summary> - 내 덱 </summary>
    public List<string> MyDeckNames
    {
        get => myDeckNames;

        set { myDeckNames = value; }
    }

    [SerializeField] private List<GameObject> myDeckGameObjects = new(20);


    async void Start()
    {
        CardDatas = await ResourceManager.Instance.AsyncRequestCardData();

        myDeckGameObjects = Resources.LoadAll<GameObject>(INGAME_CARD_PATH).ToList();

        photonView.RPC(nameof(PhotonAddCardResource), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PhotonAddCardResource()
    {
        foreach (var card in myDeckGameObjects)
        {
            PhotonNetwork.AddResource(card);
        }
    }


    /// <summary>
    /// 카드의 등급에 맞는 CardData를 반환<br></br>
    /// 없을 경우 null반환
    /// </summary>
    /// <param name="name">카드의 등급을 인자로 받음</param>
    public void TryGetCardData(string name, ref CardData cardData)
    {
        if (CardDatas.TryGetValue(name, out cardData) == false)
            print($"카드 등급이 없음 \n 카드 등급 : {name}");

        cardData = cardData.Copy();
    }

    /// <summary>
    /// 카드 등급을 카드 이름으로 하고
    /// </summary>
    /// <returns></returns>
    public string GetRandomCardName()
    {
        int randomIndex = UnityEngine.Random.Range(0, MyDeckNames.Count);
        string cardName = MyDeckNames[randomIndex];

        MyDeckNames.RemoveAt(randomIndex);

        return cardName;
    }


    /// <summary>
    /// 적이 도발 카드를 가지고 있는지 확인함 <br></br>
    /// </summary>
    /// 이 카드는 플레이어 클라이언트 기준으로 적 카드이기 때문에 적 카드 리스트에서 확인함
    public bool HasEnemyTauntCard(UnitCard card)
    {
        // 인자로 받은 카드가 Taunt 속성을 가지고 있으면
        // 공격할 수 있는 카드이니 true 반환
        if (card.CardData.CardSpecialAbilityType == CardSpecialAbilityType.Taunt)
            return true;

        // 적 카드 중에 Taunt 속성이 있는지 확인
        bool hasEnemyTauntCard =
            EnemyUnitCard.Exists(enemyCard => enemyCard.CardData.CardSpecialAbilityType == CardSpecialAbilityType.Taunt);

        // 인자로 넘긴 카드가 Taunt 속성이 아니고, 적 카드 중에도 Taunt 속성이 없으면 true 반환
        if (card.CardData.CardSpecialAbilityType != CardSpecialAbilityType.Taunt && hasEnemyTauntCard == false)
            return true;

        // 인자로 넘긴 카드가 Taunt 속성이 아니고, 적 카드 중에 Taunt 속성이 있으면 false 반환
        return false;
    }

    public void AddUnitCard(UnitCard card, bool isEnemy)
    {
        if (isEnemy)
            EnemyUnitCard.Add(card);
        else
            PlayerUnitsCard.Add(card);
    }

    public void RemoveUnitCard(UnitCard card, bool isEnemy)
    {
        if (isEnemy)
            EnemyUnitCard.Remove(card);
        else
            PlayerUnitsCard.Remove(card);

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
