using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Deck, // 덱에 있을 때
    ExpansionDeck, // 덱에 있는거 크게 볼때
    Field // 필드에 냈을 때
}

public class Card : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public CardData CardData;

    protected bool IsEnemy;
    protected bool CanDrag => IsEnemy == false && cardState == CardState.Deck;

    protected CardState cardState = CardState.Deck;
    public virtual CardState CardState { get; set; }

    protected RectTransform rect;

    private Vector2 originPos;
    private Quaternion layoutRot;
    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;
    }

    private void OnMouseEnter()
    {
        if (CanDrag == false) return;

        // 카드가 덱에 있을 때
        if (cardState == CardState.Deck)
        {
            // TODO 카드커지게 하기
            return;
        }
    }

    protected virtual void Attack(UnitCard card)
    {

    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
            layoutRot = transform.localRotation;
            transform.localRotation = Quaternion.identity;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        // 드래그할 때 포지션 바꿔줌
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rect.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        // 다시 돌아가
        transform.localRotation = layoutRot;
        rect.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        // 필드에 드롭된다면
        if (CanvasUtility.IsDropMyField())
        {
            MoveCardFromDeckToField();
            return;
        }

        var rayhits = Physics2D.RaycastAll(transform.position + Vector3.back, Vector3.forward, 10f);

        for (int i = 0; i < rayhits.Length; i++)
        {
            // 임시 공격
            if (rayhits[i].collider.TryGetComponent(out UnitCard card) && card.IsEnemy)
            {
                card.Attack(card);
            }
        }
    }

    // 원래 여기서 RPC로 호출하려고 했는데
    // 부모 클래스여서 뭔가 안되는 듯?
    // 자식 클래스로 옮기니까 잘됨
    protected virtual void MoveCardFromDeckToField()
    {
    }
}
