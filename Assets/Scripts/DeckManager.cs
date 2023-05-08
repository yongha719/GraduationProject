using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Unity.Collections;

public class DeckManager : Singleton<DeckManager> 
{
    public Canvas canvas;

    [Tooltip("카드 선택 드레그 포지션X(RectTransform)")]
    public const float standardX = 450;

    [Tooltip("모든 카드 정보")]
    public List<CardData> allCardData = new List<CardData>();

    [Tooltip("가지고 있는 카드")]
    public List<DeckBuildingCard> allHaveCardList = new List<DeckBuildingCard>();

    [Tooltip("SelectDeck")]
    public List<DeckBuildingCard> cardList = new List<DeckBuildingCard>();

    public List<DeckBuildingCard> dataList = new List<DeckBuildingCard>();

    [SerializeField]
    [Tooltip("Have카드 소환할 부모 오브젝트")]
    private Transform cardSpawnParent;

    [SerializeField]
    private Transform haveCardParent;

    public DeckBuildingCard card;

    public DragCard dragCard;

    public GameObject currentDraggingCard;


    protected override void Awake()
    {
        base.Awake();
    }

    private void LoadData()
    {
        //allCardData = Resources.LoadAll("Card")
    }
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

    }

    public void SelectCard(DeckBuildingCard data)
    {
        cardList.Add(data);
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