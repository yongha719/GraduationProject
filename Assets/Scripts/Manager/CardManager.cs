using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// 이 스크립트가 하는 역할===
// 유닛 카드 데이터들을 관리함
// 카드 드로우
// 유닛 카드들의 리스트를 관리하고 리스트들로 상호작용

public class CardManager : SingletonPunCallbacks<CardManager>, IPunObservable
{
    // 필드에 낼 수 있는 카드가 최대 10이기 때문에 Capacity를 10으로 정해줌
    public List<IUnitCardSubject> PlayerUnitCards = new(10);
    public List<IUnitCardSubject> EnemyUnitCards = new(10);


    [Header("카드 데이터들")]
    [Tooltip("유닛카드 데이터들"), SerializedDictionary("Card Rating", "Card Data")]
    public SerializedDictionary<string, CardData> CardDatas = new(30);

    // 카드의 이름으로 AddComponent를 해줘야 하기 떄문에 string으로
    // 구성해둔 덱을 받아옴
    [SerializeField]
    private List<string> cardNames = new(20);

    /// <summary> - 내 덱 </summary>
    [field: SerializeField]
    public List<string> CardNames
    {
        get => cardNames;

        set => cardNames = value;
    }

    public bool HasEnemyTauntCard => enemyTauntCardCount != 0;

    [Tooltip("적 도발 카드 갯수")]
    private uint enemyTauntCardCount = 0;

    /// <summary> 인게임 카드 경로 </summary>
    /// 포톤에서 Resource.Load를 사용하기 때문에
    /// 이런식으로 경로를 정의함
    private const string CARD_PATH = "Cards/In Game Card";


    private async void Start()
    {
        CardDatas = await ResourceManager.Instance.GetCardDatas();
    }

    /// <summary>
    /// 카드의 등급에 맞는 CardData를 반환<br></br>
    /// 없을 경우 null반환
    /// </summary>
    /// <param name="name">카드의 등급을 인자로 받음</param>
    public void TryGetCardData<T>(string name, ref T unitCardData) where T : CardData
    {
        if (CardDatas.TryGetValue(name, out CardData data))
            unitCardData = (T)data.Copy();
        else
            Debug.Assert(false, $"카드 등급이 없음 \n 카드 등급 : {name}");
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CardDraw();
#endif
    }

    /// <summary>
    /// 카드 등급을 카드 이름으로 했음
    /// </summary>
    public string GetRandomCardType()
    {
        int randomIndex = Random.Range(0, CardNames.Count);
        string cardName = CardNames[randomIndex];

        CardNames.RemoveAt(randomIndex);

        return cardName;
    }

    public void CardDraw(int count)
    {
        for (int i = 0; i < count; i++)
            CardDraw();
    }

    public GameObject CardDraw()
    {
        return CardDraw(GetRandomCardType(), isPlayeDraw: true, setParentAsDeck: true);
    }

    public GameObject CardDraw(string cardName, bool isPlayeDraw = true)
    {
        return CardDraw(cardName, isPlayeDraw, setParentAsDeck: true);
    }

    public GameObject CardDraw(string cardName, bool isPlayeDraw = true, bool setParentAsDeck = true)
    {
        var cardObj = PhotonNetwork.Instantiate(CARD_PATH, Vector2.zero, Quaternion.identity);

        PhotonView cardPhotonView = cardObj.GetPhotonView();

        if (PhotonManager.IsAlone)
            SetCardAndParentRPC(cardName, cardPhotonView.ViewID, isPlayeDraw, setParentAsDeck);
        else
            photonView.RPC(nameof(SetCardAndParentRPC), RpcTarget.AllBuffered,
                cardName, cardPhotonView.ViewID, isPlayeDraw, setParentAsDeck);

        return cardObj;
    }

    /// <summary>
    /// 카드 이름과 필드 세팅
    /// </summary>
    /// RPC 호출이라 다른 클라이언트에서는 어떤 개체인지 모르기 때문에
    /// viewId로 찾아야 하기 때문에 인자로 넘겨줘야함
    [PunRPC]
    private void SetCardAndParentRPC(string cardName, int cardViewId, bool isPlayeDraw = true, bool setParentAsDeck = true)
    {
        PhotonView cardPhotonView = PhotonManager.GetPhotonView(cardViewId);

        cardPhotonView.gameObject.name = cardName;

        Component cardType = null;

        cardName = $"{cardName}Card";
        cardType = cardPhotonView.gameObject.AddComponent(Type.GetType(cardName));

        PhotonView parentPhotonView;

        if (setParentAsDeck)
        {
            parentPhotonView = PhotonManager.GetPhotonViewByViewType(isPlayeDraw
                ? PhotonViewType.PlayerDeck
                : PhotonViewType.EnemyDeck);
        }
        else
        {
            parentPhotonView = PhotonManager.GetPhotonViewByViewType(isPlayeDraw
                ? PhotonViewType.PlayerField
                : PhotonViewType.EnemyField);
        }

        cardPhotonView.transform.SetParent(parentPhotonView.transform);

        var card = cardType as Card;

        bool cardIsNull = card == null;

        if (cardIsNull == false)
            card.Init(isPlayeDraw, parentPhotonView.transform);
        else
        {
            Debug.Assert(false,
                $"뭔가 잘못됨\n {nameof(CardDeckLayout)} : 카드 이름: {cardName}, 카드 is null :{cardIsNull}");
        }
    }


    public void AddUnitCard(UnitCard card)
    {
        if (card.IsEnemy)
        {
            EnemyUnitCards.Add(card);

            if (card.CardData.UnitCardSpecialAbilityType == UnitCardSpecialAbilityType.Taunt)
                enemyTauntCardCount++;
        }
        else
            PlayerUnitCards.Add(card);
    }

    public void RemoveUnitCard(UnitCard card)
    {
        if (card.IsEnemy)
        {
            EnemyUnitCards.Remove(card);

            if (card.CardData.UnitCardSpecialAbilityType == UnitCardSpecialAbilityType.Taunt)
                enemyTauntCardCount--;
        }
        else
            PlayerUnitCards.Remove(card);

        CardDestroy(card);
    }

    public void HandleCards(bool myTurn)
    {
        var cards = myTurn ? PlayerUnitCards : EnemyUnitCards;

        cards.ForEach(card => card.HandleTurn());
    }

    public void HealCards(int healAmount)
    {
        foreach (var card in PlayerUnitCards)
        {
            card.HealCard(healAmount);
        }
    }

    public void AttackEnemyCards(int damage)
    {
        EnemyUnitCards.ForEach(card => card.Hit(damage));
    }

    private void CardDestroy(UnitCard unitCard)
    {
        unitCard.gameObject.SetActive(false);

        if (!unitCard.IsEnemy)
            PhotonNetwork.Destroy(unitCard.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}