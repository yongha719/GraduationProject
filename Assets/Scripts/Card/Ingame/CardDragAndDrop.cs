using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragAndDrop : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action OnEndDrag;
    public event Action OnDrop;

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    public bool CanDrag => isEnemy == false && cardState != CardState.Field && TurnManager.Instance.MyTurn;

    private bool IsDragging;

    private bool isEnemy;

    private CardState cardState
    {
        get => card.CardState;
        set => card.CardState = value;
    }

    private Vector2 originPos;
    [Tooltip("덱 레이아웃이 회전값")] private Quaternion layoutRot;

    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    private LineRenderer lineRenderer;
    private RectTransform rectTransform;

    private Card card;

    private void Start()
    {
        isEnemy = !photonView.IsMine;

        rectTransform = transform as RectTransform;
        lineRenderer = GetComponent<LineRenderer>();

        card = GetComponent<Card>();
    }

    private void OnMouseEnter()
    {
        print(nameof(OnMouseEnter));

        // 카드가 덱에 있을 때 카드에 마우스 올리면
        if (CanDrag && cardState == CardState.Deck && IsDragging == false)
        {
            cardState = CardState.ExpansionDeck;
        }
    }

    private void OnMouseExit()
    {
        print(nameof(OnMouseExit));

        if (CanDrag && cardState == CardState.ExpansionDeck && IsDragging == false)
        {
            cardState = CardState.Deck;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        print("OnBeginDrag");
        
        if (cardState == CardState.Field)
        {
            lineRenderer.SetPosition(0, (Vector3)CanvasUtility.GetMousePosToCanvasPos() + Vector3.back);
        }

        if (CanDrag == false) return;

        IsDragging = true;

        if (cardState == CardState.ExpansionDeck)
            cardState = CardState.Deck;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rectTransform.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();

            // 드래그 할 때는 카드가 돌아가 있으면 안됨
            layoutRot = transform.localRotation;
            transform.localRotation = Quaternion.identity;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (cardState == CardState.Field)
        {
            lineRenderer.SetPosition(1, (Vector3)CanvasUtility.GetMousePosToCanvasPos() + Vector3.back);
        }

        if (CanDrag == false) return;

        // 드래그할 때 포지션 바꿔줌
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rectTransform.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        print("OnEndDrag");
        

        if (CanDrag == false) return;

        IsDragging = false;

        // 다시 돌아가
        transform.localRotation = layoutRot;
        rectTransform.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        OnDrop();
    }
}