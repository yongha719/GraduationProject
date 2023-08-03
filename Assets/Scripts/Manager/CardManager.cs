using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// 이 스크립트가 하는 역할===
// 유닛 카드 데이터들을 관리함
// 카드 드로우
// 유닛 카드들의 리스트를 관리하고 리스트들로 상호작용

public class CardManager : SingletonPunCallbacks<CardManager>, IPunObservable
{
    // 필드에 낼 수 있는 카드가 최대 10이기 때문에 Capacity를 10으로 정해줌
    public List<UnitCard> PlayerUnitCards = new(10);
    public List<UnitCard> EnemyUnitCards = new(10);


    [Header("카드 데이터들")]
    [Tooltip("유닛카드 데이터들"), SerializedDictionary("Card Rating", "Card Data")]
    private SerializedDictionary<string, CardData> CardDatas = new(30);

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

    public List<string> CardDataNames;

    public bool HasEnemyTauntCard => enemyTauntCardCount != 0;

    [Tooltip("적 도발 카드 갯수")]
    private uint enemyTauntCardCount = 0;

    /// <summary> 인게임 카드 경로 </summary>
    /// 포톤에서 Resource.Load를 사용하기 때문에
    /// 이런식으로 경로를 정의함
    private const string CARD_PATH = "Cards/In Game Card";

    [Header("마법 카드 스킬들")]
    
    [Tooltip("상대가 유닛 카드를 냈을 때 복사")]
    public bool ShouldSummonCopy;

    [Tooltip("지뢰 마법카드 썼는지 체크")]
    public bool UseMineMasic;

    private const int MINE_DAMAGE = 5;

    [Tooltip("")]
    public bool CanUseMasicCard = true;
    
    private async void Start()
    {
        CardDatas = await ResourceManager.Instance.GetCardDatas();

        CardDataNames = new List<string>(CardDatas.Keys);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CardDraw();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("press 2");
            print(PlayerUnitCards.Count);
            print(EnemyUnitCards.Count);
            
            foreach (var card in PlayerUnitCards)
            {
                TurnManager.Instance.ExecuteAfterTurn(1,
                    () =>
                    {
                        print(card.name);
                    });
            }
        }
#endif
    }

    [PunRPC]
    private void Test()
    {
        CanUseMasicCard = false;
        
    }
    
    /// <summary>
    /// 카드의 등급에 맞는 CardData를 반환<br></br>
    /// 없을 경우 null반환
    /// </summary>
    /// <param name="name">카드의 등급을 인자로 받음</param>
    public void TryGetCardData<T>(string name, ref T CardData) where T : CardData
    {
        if (CardDatas.TryGetValue(name, out CardData data))
            CardData = (T)data.Copy();
        else
            Debug.Assert(false, $"카드 등급이 없음 \n카드 등급 : {name}");
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
        return CardDraw(GetRandomCardType(), isPlayeDraw: false, setParentAsDeck: true);
    }

    public GameObject CardDraw(string cardName, bool isPlayeDraw = false)
    {
        return CardDraw(cardName, isPlayeDraw, setParentAsDeck: true);
    }

    public GameObject CardDraw(string cardName, bool isPlayeDraw, bool setParentAsDeck = true)
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
    private void SetCardAndParentRPC(string cardName, int cardViewId, bool isPlayeDraw = false,
        bool setParentAsDeck = true)
    {
        PhotonView cardPhotonView = PhotonManager.GetPhotonView(cardViewId);

        cardPhotonView.gameObject.name = cardName;

        cardName = $"{cardName}Card";

        Component cardType = cardPhotonView.gameObject.AddComponent(Type.GetType(cardName));

        PhotonView parentPhotonView;

        // 카드를 덱에 소환하는지 확인 후
        if (setParentAsDeck)
        {
            // 카드가 플레이어의 덱에 들어와야하는지 확인
            if (isPlayeDraw)
                parentPhotonView = PhotonManager.GetPhotonViewByViewType(PhotonViewType.PlayerDeck);
            else
                parentPhotonView = PhotonManager.GetDeckPhotonView(cardPhotonView.IsMine);
        }
        else
        {
            // 카드가 플레이어의 필드에 소환돼야하는지 확인
            if (isPlayeDraw)
                parentPhotonView = PhotonManager.GetPhotonViewByViewType(PhotonViewType.PlayerField);
            else
                parentPhotonView = PhotonManager.GetFieldPhotonView(cardPhotonView.IsMine);
        }

        cardPhotonView.transform.SetParent(parentPhotonView.transform);

        if (cardType is Card card)
        {
            card.Init(isPlayeDraw ? isPlayeDraw : cardPhotonView.IsMine, parentPhotonView.transform);
            print("isMine : " + (isPlayeDraw ? isPlayeDraw : cardPhotonView.IsMine));
        }
        else
            Debug.Assert(false,
                $"뭔가 잘못됨\n {nameof(CardDeckLayout)} : 카드 이름: {cardName}, 카드 is null :{cardType == null}");
    }


    public void AddUnitCard(UnitCard card)
    {
        if (card.IsMine)
            PlayerUnitCards.Add(card);
        else
        {
            EnemyUnitCards.Add(card);

            if (card.CardData.UnitCardSpecialAbilityType == UnitCardSpecialAbilityType.Taunt)
                enemyTauntCardCount++;

            if (ShouldSummonCopy)
            {
                string cardName = card.name.Split("_")[0];

                var copyCard = CardDraw(cardName, isPlayeDraw: false, setParentAsDeck: false);
                copyCard.GetComponent<UnitCard>().CardState = CardState.Field;

                print("적 카드 소환함 : " + cardName);
                ShouldSummonCopy = false;
            }

            if (UseMineMasic)
            {
                // 지뢰썼을때 이펙트
                
                card.Hit(MINE_DAMAGE);

                UseMineMasic = false;
            }
        }
    }

    public void RemoveUnitCard(UnitCard card)
    {
        if (card.IsMine)
            PlayerUnitCards.Remove(card);
        else
        {
            EnemyUnitCards.Remove(card);

            if (card.CardData.UnitCardSpecialAbilityType == UnitCardSpecialAbilityType.Taunt)
                enemyTauntCardCount--;
        }

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

        if (unitCard.IsMine)
            PhotonNetwork.Destroy(unitCard.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}