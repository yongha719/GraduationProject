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
    public string GetRandomCardName()
    {
        int randomIndex = Random.Range(0, MyDeckNames.Count);
        string cardName = MyDeckNames[randomIndex];

        MyDeckNames.RemoveAt(randomIndex);

        return cardName;
    }

    public void AddUnitCard<T>(T card) where T : UnitCard, IUnitCardSubject
    {
        if (card.IsEnemy)
        {
            EnemyUnitCards.Add(card);

            if (card.CardData.CardSpecialAbilityType == CardSpecialAbilityType.Taunt)
                tauntCardCount++;
        }
        else
            PlayerUnitCards.Add(card);
    }

    public void RemoveUnitCard<T>(T card) where T : UnitCard, IUnitCardSubject
    {
        if (card.IsEnemy)
        {
            EnemyUnitCards.Remove(card);

            if (card.CardData.CardSpecialAbilityType == CardSpecialAbilityType.Taunt)
                tauntCardCount--;
        }
        else
            PlayerUnitCards.Remove(card);

        card.Destroy();
    }

    public void TurnChange(bool myTurn)
    {
        var cards = myTurn ? PlayerUnitCards : EnemyUnitCards;

        cards.ForEach(card => card.HandleTurn());
    }

    public void HealCards(int healAmount)
    {
        PlayerUnitCards.ForEach(card => HealCards(healAmount));
    }

    public void AttackEnemyCards(int damage)
    {
        EnemyUnitCards.ForEach(card => card.Hit(damage));

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
