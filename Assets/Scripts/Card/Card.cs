#if UNITY_EDITOR
#define DDDD
#endif

using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Deck, Field
}

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    private CardState cardState;
    public CardState CardState
    {
        get
        {
            return cardState;
        }
        set
        {
            cardState = value;

            if (cardState == CardState.Deck)
            {
                rect.localScale = Vector3.one;
            }
            else if (cardState == CardState.Field)
            {
                rect.localScale = Vector3.one * 0.6f;
            }
        }
    }

    public CardData cardData;
    public bool IsEnemy;

    Vector2 originPos;
    /// <summary>클릭했을때 마우스 포인터와 카드 중앙에서의 거리</summary>
    Vector2 mousePosDistance;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    private void OnMouseEnter()
    {
        // 카드가 덱에 있을 때
        if (cardState == CardState.Deck)
        {
            // TODO 카드커지게 하기
            return;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // 드래그할 때 포지션 바꿔줌
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rect.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        // 다시 돌아가
        rect.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var rayhits = Physics2D.RaycastAll(transform.position + Vector3.back, Vector3.forward, 10f);

        for (int i = 0; i < rayhits.Length; i++)
        {
            if (rayhits[i].collider.TryGetComponent(out Card card) && card.IsEnemy)
            {
                print("드디어");
            }
        }
    }
}
