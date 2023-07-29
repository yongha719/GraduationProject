using System;
using Photon.Pun;
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

    [field: SerializeField]
    public Vector3 OriginPos { get; private set; }

    [Tooltip("덱 레이아웃이 회전값")]
    public Quaternion layoutRot;

    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    [SerializeField]
    private int silblingIndex;

    private Rect rect;

    [SerializeField, Tooltip("3D 카드 효과 회전값")]
    private float cardRotationValue;

    [SerializeField, Tooltip("3D 카드 효과 그림자 크기값")]
    private float effectDistanceValue;

    private RectTransform rectTransform;
    private Shadow shadow;

    private Card card;

    private void Start()
    {
        rectTransform = transform as RectTransform;

        shadow = GetComponent<Shadow>();

        rect = rectTransform.rect;
        rect.max *= 1.5f;
        rect.min *= 1.5f;

        MoveCardToFieldAction += () =>
        {
            if (cardState == CardState.Field)
                shadow.enabled = false;
        };
    }

    public void Init()
    {
        card = GetComponent<Card>();

        isEnemy = card.IsEnemy;
        if (isEnemy)
            shadow.enabled = false;
    }

    private void OnMouseEnter()
    {
        if (cardState == CardState.Deck && isEnemy == false && hasExpansionCard == false && isDragging == false)
        {
            print($"hasExpansionCard : {hasExpansionCard}");
            layoutRot = rectTransform.localRotation;

            cardState = CardState.ExpansionDeck;

            silblingIndex = rectTransform.GetSiblingIndex();
            rectTransform.SetAsLastSibling();
        }
    }

    private void OnMouseOver()
    {
        if (cardState == CardState.Field || isEnemy)
            return;

        Vector2 mousePosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, Camera.main,
            out Vector2 localPoint);

        if (rect.Contains(localPoint))
        {
            // rect 회전값
            Vector3 rotation =
                CanvasUtility.LerpVector2InRect(cardRotationValue, -cardRotationValue, rect, localPoint);

            rectTransform.rotation = Quaternion.Euler(rotation);

            // Shadow effectDistance 값
            Vector2 effectDistance =
                CanvasUtility.LerpVector2InRect(effectDistanceValue, -effectDistanceValue, rect, localPoint);

            shadow.effectDistance = effectDistance;
        }
    }
    

    private void OnMouseExit()
    {
        if (isEnemy == false && cardState == CardState.ExpansionDeck && photonView.IsMine)
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

        hasExpansionCard = false;
        isDragging = true;

        if (cardState == CardState.ExpansionDeck)
            cardState = CardState.Deck;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OriginPos = rectTransform.anchoredPosition3D;
            mousePosDistance = OriginPos - (Vector3)CanvasUtility.GetMousePosToCanvasPos();

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
        transform.localRotation = cardState != CardState.Field ? layoutRot : Quaternion.identity;
        rectTransform.anchoredPosition3D = OriginPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        OnDrop();

        if (cardState != CardState.Field)
        {
            // TODO : 여기서 코스트 감소
            if (GameManager.Instance.CheckCardCostAvailability((uint)card.Cost, out Action costDecrease) == false &&
                CanvasUtility.IsDropMyField())
                return;

            MoveCardToFieldAction();
        }
    }
}