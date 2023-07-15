using Photon.Pun;
using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragAndDrop : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action OnEndDrag;
    public event Action OnDrop = () => { };
    public event Action MoveCardToFieldAction = () => { };

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    public bool CanDrag => isEnemy == false && TurnManager.Instance.MyTurn;

    private static bool hasExpansionCard;
    private bool isDragging;

    private bool isEnemy;

    private CardState cardState
    {
        get => card.CardState;
        set => card.CardState = value;
    }

    private Vector3 originPos;
    public Vector3 OriginPos => originPos;

    [Tooltip("덱 레이아웃이 회전값")] public Quaternion layoutRot;

    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    private RectTransform rectTransform;
    private Shadow shadow;

    private Card card;

    private Vector2 rectMax;
    private Vector2 rectMin;

    private int silblingIndex;

    private void Start()
    {
        isEnemy = !photonView.IsMine;

        rectTransform = transform as RectTransform;

        shadow = GetComponent<Shadow>();

        rectMax = rectTransform.rect.max * 1.5f;
        rectMin = rectTransform.rect.min * 1.5f;

        MoveCardToFieldAction += () =>
        {
            if (cardState == CardState.Field)
                shadow.enabled = false;
        };
    }

    public void Init()
    {
        card = GetComponent<Card>();

        if (isEnemy)
            shadow.enabled = false;
    }

    private void OnMouseEnter()
    {
        switch (cardState)
        {
            case CardState.Deck:
                if (isEnemy == false && hasExpansionCard == false && isDragging == false)
                {
                    layoutRot = rectTransform.localRotation;

                    cardState = CardState.ExpansionDeck;

                    silblingIndex = rectTransform.GetSiblingIndex();
                    rectTransform.SetAsLastSibling();
                }

                break;
            case CardState.ExpansionDeck:
                break;
            case CardState.Field:
                shadow.enabled = false;
                break;
        }
    }

    private void OnMouseOver()
    {
        if (cardState == CardState.Field || isEnemy)
            return;

        hasExpansionCard = true;

        Vector2 mousePosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, Camera.main,
            out Vector2 localPoint);

        if (RectangleContainsScreenPoint(rectTransform, mousePosition, Camera.main, localPoint))
        {
            // rect 회전값
            Vector3 rotation = Vector3.one;

            rotation.y = Mathf.Lerp(-25, 25, MapToZeroToOneX(localPoint.x));
            rotation.x = Mathf.Lerp(25, -25, MapToZeroToOneY(localPoint.y));

            rectTransform.rotation = Quaternion.Euler(rotation);

            // Shadow effectDistance 값
            Vector2 effectDistance;

            effectDistance.x = Mathf.Lerp(-10, 10, MapToZeroToOneX(localPoint.x));
            effectDistance.y = Mathf.Lerp(-10, 10, MapToZeroToOneY(localPoint.y));

            shadow.effectDistance = effectDistance;
        }
    }

    // 1.5 배 큰 범위로 잡고 싶어서 만들었음
    private bool RectangleContainsScreenPoint(RectTransform rectTransform, Vector2 screenPoint, Camera camera,
        Vector2 rectPosition)
    {
        // 로컬 좌표가 RectTransform의 경계 내에 있는지 확인합니다.
        Rect rect = rectTransform.rect;

        rect.max *= 1.5f;
        rect.min *= 1.5f;

        return rect.Contains(rectPosition);
    }

    private void OnMouseExit()
    {
        if (isEnemy == false && cardState == CardState.ExpansionDeck && hasExpansionCard && photonView.IsMine)
        {
            cardState = CardState.Deck;

            rectTransform.localRotation = layoutRot;
            rectTransform.SetSiblingIndex(silblingIndex);

            hasExpansionCard = false;

            shadow.effectDistance = Vector2.zero;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        isDragging = true;

        if (cardState == CardState.ExpansionDeck)
            cardState = CardState.Deck;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rectTransform.anchoredPosition3D;
            mousePosDistance = originPos - (Vector3)CanvasUtility.GetMousePosToCanvasPos();

            // 드래그 할 때는 카드가 돌아가 있으면 안됨
            rectTransform.localRotation = Quaternion.identity;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        // 드래그할 때 포지션 바꿔줌
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rectTransform.anchoredPosition3D = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        isDragging = false;

        // 다시 돌아가
        // transform.localRotation = cardState != CardState.Field ? layoutRot : Quaternion.identity;
        rectTransform.anchoredPosition3D = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        hasExpansionCard = false;
        OnDrop();

        if (cardState != CardState.Field)
        {
            // TODO : 여기서 코스트 감소
            if (GameManager.Instance.CheckCardCostAvailability((uint)card.Cost, out Action costDecrease) == false)
                return;

            costDecrease();

            MoveCardToFieldAction();
        }
    }

    float MapToZeroToOneX(float value)
    {
        // 상대적인 위치 계산
        float relativePosition = (value - rectMin.x) / (rectMax.x - rectMin.x);

        // 0과 1 사이의 범위로 매핑
        float mappedValue = Mathf.Clamp01(relativePosition);

        return mappedValue;
    }

    float MapToZeroToOneY(float value)
    {
        // 상대적인 위치 계산
        float relativePosition = (value - rectMin.y) / (rectMax.y - rectMin.y);

        // 0과 1 사이의 범위로 매핑
        float mappedValue = Mathf.Clamp01(relativePosition);

        return mappedValue;
    }
}