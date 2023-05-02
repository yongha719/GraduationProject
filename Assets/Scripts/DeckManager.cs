using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [Tooltip("카드 선택 드레그 포지션X(RectTransform)")]
    public const float standardX = 450;

    [Tooltip("카드 정렬 간격")]
    [HideInInspector]
    public Vector2 spacing = new Vector2(300, 300);

    [Tooltip("Have카드 가로 최대 숫자")]
    public const int MAXHORIZONTALCOUNT = 4;

    [Tooltip("가지고 있는 카드")]
    public List<CardDeck> allHaveCardList = new List<CardDeck>();

    [Tooltip("SelectDeck")]
    public List<CardDeck> cardList = new List<CardDeck>();

    public List<CardData> dataList = new List<CardData>();

    public Sprite[] sprites = new Sprite[30];

    [Tooltip("시작 지점")]
    public Vector3 startSortPosition;

    [SerializeField]
    private Transform cardSpawnParent;

    [SerializeField]
    private Transform haveCardParent;

    public CardDeck card;

    private void Start()
    {
        SetHaveCards();
    }

    private void SetHaveCards()
    {
        for (int i = 0; i < allHaveCardList.Count; i++)
        {

        }
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

    /// <summary>
    /// PutOn카드 정렬
    /// </summary>
    /// <param name="list"></param>
    private void SortDeck(List<CardData> list)
    {
        list.OrderBy(item => item.Cost);
    }

    /// <summary>
    /// Have카드 위치 정렬
    /// </summary>
    public void SortCard()
    {
        for (int i = 0; i < allHaveCardList.Count; i++)
        {
            allHaveCardList[i].transform.position = 
                new Vector2((startSortPosition.x + allHaveCardList.Count) % MAXHORIZONTALCOUNT * spacing.x
                ,(startSortPosition.y - i) * spacing.y);
        }
    }
}