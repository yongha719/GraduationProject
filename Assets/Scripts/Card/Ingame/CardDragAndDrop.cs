using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragAndDrop : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action OnEndDrag;
    public event Action OnDrop;

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    public bool CanDrag => isEnemy == false && TurnManager.Instance.MyTurn;

    private bool isDragging;

    private bool isEnemy;

    private CardState cardState
    {
        get => card.CardState;
        set => card.CardState = value;
    }

    private Vector2 originPos;
    [Tooltip("덱 레이아웃이 회전값")] public Quaternion layoutRot;

    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    private RectTransform rectTransform;

    private Card card;

    private void Start()
    {
        isEnemy = !photonView.IsMine;

        rectTransform = transform as RectTransform;
    }

    public void Init()
    {
        card = GetComponent<Card>();
    }
    
    private void OnMouseEnter()
    {
        print("mouse enter");

        switch (cardState)
        {
            case CardState.Deck:
                if (CanDrag && isDragging == false)
                {
                    cardState = CardState.ExpansionDeck;
                    layoutRot = rectTransform.localRotation;
                    rectTransform.localRotation = Quaternion.identity;
                }

                break;
            case CardState.ExpansionDeck:
                break;
            case CardState.Field:
                if (CanDrag)
                {
                }

                break;
        }
    }

    private void OnMouseExit()
    {
        if (CanDrag && cardState == CardState.ExpansionDeck && isDragging == false)
        {
            cardState = CardState.Deck;
            rectTransform.localRotation = layoutRot;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (cardState == CardState.Field)
        {
            // lineRenderer.SetPosition(0, (Vector3)CanvasUtility.GetMousePosToCanvasPos() + Vector3.back);
        }

        if (CanDrag == false) return;

        isDragging = true;

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
            // lineRenderer.SetPosition(1, (Vector3)CanvasUtility.GetMousePosToCanvasPos() + Vector3.back);
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
        // TODO : 여기서 코스트 감소
        
        if (CanDrag == false) return;

        isDragging = false;

        // 다시 돌아가
        transform.localRotation = cardState != CardState.Field ? layoutRot : Quaternion.identity;
        rectTransform.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        OnDrop();
    }
}