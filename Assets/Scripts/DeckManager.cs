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
    public List<DeckBuildingCard> selectedCardList = new List<DeckBuildingCard>();

    [SerializeField]
    [Tooltip("덱 편성 ")]
    private List<GameObject> selectStateCardObjList = new List<GameObject>();

    [SerializeField]
    [Tooltip("SelectCard부모 개체")]
    private Transform selectCardParent;

    [SerializeField]
    [Tooltip("HaveCard부모 개체")]
    private Transform haveCardParent;

    public DeckBuildingCard buildingCard;

    [Tooltip("prefab")]
    public DragCard dragCard;

    [Tooltip("지금 드레그 중인 카드")]
    public DragCard currentDraggingCard;

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

    public void SelectCard(CardData data)
    {
        DeckBuildingCard tempCard = Instantiate(buildingCard, selectCardParent);
        tempCard.data = data;
        tempCard.IsSelect = true;
        selectedCardList.Add(tempCard);
    }

    public DragCard SpawnDragCard(CardData data)
    {
        DragCard card = Instantiate(dragCard, canvas.transform);
        //card.SetDragCard(data);

        return card;
    }

    public DragCard SpawnDragCardTemp(Vector3 pos)
    {
        DragCard card = Instantiate(dragCard, canvas.transform);
        card.transform.position = pos;
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