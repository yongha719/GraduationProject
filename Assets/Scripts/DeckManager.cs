using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [Tooltip("기준 X 값")]
    public float standardX;

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