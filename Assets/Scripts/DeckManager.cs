using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Unity.Collections;
using UnityEngine.UI;

public class DeckManager : Singleton<DeckManager>
{
    public Canvas canvas;

    [Tooltip("카드 선택 드레그 포지션X(RectTransform)")]
    public const float standardX = 450;

    [Tooltip("모든 카드 정보")]
    public List<CardData> allCardData = new List<CardData>();

    #region 카드 List들
    [Header("카드List변수들")]
    [Tooltip("간격")]
    public float spacing;

    [Tooltip("가지고 있는 카드")]
    public List<DeckBuildingCard> allHaveCardList = new List<DeckBuildingCard>();

    [Tooltip("SelectDeck")]
    public List<DeckBuildingCard> selectedCardList = new List<DeckBuildingCard>();

    #endregion

    public List<DeckBuildingCard> cardlist
    {
        get => allHaveCardList;
        set
        {
            allHaveCardList = value;
            

        }
    }

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

    public void DeSelectCard(DeckBuildingCard card)
    {
        //selectedCardList.Find(card);
        selectedCardList.Remove(card);
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


    public void SortSelectDeck()
    {
        // 정렬할 개체들을 가져온다 (예: 자식 개체들)
        DeckBuildingCard[] childObjects = selectCardParent.GetComponentsInChildren<DeckBuildingCard>();

        // 개체들을 원하는 값으로 정렬한다
        SortObjects(childObjects);

        // 개체들의 위치를 정렬한다
        //IntervalAdjustment(childObjects);

        // Layout Group을 갱신하여 정렬을 적용한다
        LayoutRebuilder.ForceRebuildLayoutImmediate(selectCardParent.GetComponent<RectTransform>());
    }
    
    private void SortObjects(DeckBuildingCard[] objects)
    {
        // 개체들을 정렬하는 로직
        for (int i = 0; i < objects.Length - 1; i++)
        {
            if (objects[i].data.Cost > objects[i + 1].data.Cost)
            {
                Swap(objects[i], objects[i + 1]);
            }
        }
    }

    private void IntervalAdjustment(DeckBuildingCard[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.localPosition = new Vector3(0, spacing * i, 0);
        }
    }

    private void Swap(DeckBuildingCard g1, DeckBuildingCard g2)
    {
        DeckBuildingCard temp = g1;

        g1 = g2;

        g2 = temp;
    }
}