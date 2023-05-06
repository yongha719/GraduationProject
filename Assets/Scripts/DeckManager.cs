using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class DeckManager : Singleton<DeckManager>
{
    public Canvas canvas;

    [Tooltip("카드 선택 드레그 포지션X(RectTransform)")]
    public const float standardX = 450;

    [Tooltip("가지고 있는 카드")]
    public List<CardDeck> allHaveCardList = new List<CardDeck>();

    [Tooltip("SelectDeck")]
    public List<CardDeck> cardList = new List<CardDeck>();

    public List<CardData> dataList = new List<CardData>();

    [SerializeField]
    [Tooltip("Have카드 소환할 부모 오브젝트")]
    private Transform cardSpawnParent;

    [SerializeField]
    private Transform haveCardParent;

    public CardDeck card;

    public DragCard dragCard;

    public GameObject currentDraggingCard;

    private void Start()
    {
        SetHaveCards();
    }

    private void SetHaveCards()
    {

    }

    private void Update()
    {
        
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            
        }
    }

    public void SelectCard(CardData data)
    {

    }

    public DragCard SpawnCardDeck(CardData data)
    {
        DragCard card = Instantiate(dragCard, canvas.transform);
        

        return card;
    }

    /// <summary>
    /// PutOn카드 정렬
    /// </summary>
    /// <param name="list"></param>
    private void SortDeck(List<CardData> list)
    {
        list.OrderBy(item => item.Cost);
    }
}