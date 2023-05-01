using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [Tooltip("카드 선택 드레그 포지션X(RectTransform)")]
    public const float standardX = 450;

    [Tooltip("카드 정렬 간격")]
    public static Vector2 Spacing = new Vector2(300, 300);

    [Tooltip("카드 가로 최대 숫자")]
    public const int MAXHORIZONTALCOUNT = 4;

    [Tooltip("가지고 있는 카드")]
    public List<CardDeck> allHaveCardList = new List<CardDeck>();

    [Tooltip("deck")]
    public List<CardDeck> cardList = new List<CardDeck>();

    [Tooltip("시작 지점")]
    public Vector3 startSortPosition;

    [SerializeField]
    private Transform cardSpawnParent;

    public CardDeck card;

    private void Start()
    {

    }

    public void SelectCard(CardData data)
    {

    }

    private void SortDeck(List<CardData> list)
    {
        list.OrderBy(item => item.Cost);
    }

    public void SortCard()
    {
        for (int i = 0; i < allHaveCardList.Count; i++)
        {
            allHaveCardList[i].transform.position = new Vector2(allHaveCardList.Count % MAXHORIZONTALCOUNT * Spacing.x, i * Spacing.y);
        }
    }
}