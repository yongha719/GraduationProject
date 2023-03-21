using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Tooltip("±âÁØ X °ª")]
    public float standardX;

    public List<CardDeck> allHaveCardList = new List<CardDeck>();

    public List<CardDeck> deckList = new List<CardDeck>();

    [SerializeField]
    private Transform cardSpawnPos;

    private void Start()
    {
        
    }

    public void AddCard(CardData data)
    {

    }


}
