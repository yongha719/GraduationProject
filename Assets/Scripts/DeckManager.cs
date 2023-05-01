using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [Tooltip("카드 선택 드레그 포지션X(RectTransform)")]
    public const float standardX = 450;

    public List<CardData> allHaveCardList = new List<CardData>();

    public List<CardData> cardList = new List<CardData>();

    [SerializeField]
    private Transform cardSpawnParent;

    public CardDeck card;

    private void Start()
    {
        
    }

    public void SelectCard(CardData data)
    {
        cardList.Add(data);
        SortDeck(cardList);
    }

    private void SortDeck(List<CardData> list)
    {
        list.OrderBy(item => item.Cost);
    }
}