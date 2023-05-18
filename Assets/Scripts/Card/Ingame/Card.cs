using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[Serializable]
public enum CardState
{
    Deck, // 덱에 있을 때
    ExpansionDeck, // 덱에 있는거 크게 볼때
    Field // 필드에 냈을 때
}

[Serializable]
/// <summary> 인게임 카드의 부모 클래스 </summary>
public abstract class Card : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public CardData CardData;

    protected bool IsEnemy;

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    protected bool CanDrag => IsEnemy == false && CardState == CardState.Deck && TurnManager.Instance.MyTurn;

    /// <summary> 공격 가능한 상태인지 체크 </summary>
    protected bool CanAttack => IsEnemy == false && CardState == CardState.Field && TurnManager.Instance.MyTurn;


    protected CardState cardState = CardState.Deck;
    public virtual CardState CardState { get; set; }

    protected RectTransform rect;
    protected LineRenderer lineRenderer;

    private Vector2 originPos;
    [Tooltip("덱 레이아웃이 회전값")]
    private Quaternion layoutRot;
    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    protected virtual void Awake()
    {
        // 카드 오브젝트의 이름은 카드의 등급으로 되어있기 때문에 이름을 카드 등급만 남게해줌
        name = name.Replace("(Clone)", "");

        rect = GetComponent<RectTransform>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;


    }

    private void OnMouseEnter()
    {
        if (CanDrag == false) return;

        // 카드가 덱에 있을 때 카드에 마우스 올리면
        if (cardState == CardState.Deck)
        {
            // TODO 카드커지게 하기
            return;
        }
    }


    protected virtual void Attack()
    {
    }



    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (CardState == CardState.Field)
        {
            lineRenderer.SetPosition(0, (Vector3)CanvasUtility.GetMousePosToCanvasPos() + Vector3.back);
        }

        if (CanDrag == false) return;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();

            // 드래그 할 때는 카드가 돌아가 있으면 안됨
            layoutRot = transform.localRotation;
            transform.localRotation = Quaternion.identity;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (CardState == CardState.Field)
        {
            lineRenderer.SetPosition(1, (Vector3)CanvasUtility.GetMousePosToCanvasPos() + Vector3.back);
        }

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
        if (CanAttack)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        if (CanDrag == false) return;


        // 다시 돌아가
        transform.localRotation = layoutRot;
        rect.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Attack();

        if (CanDrag == false) return;

        // 필드에 드롭된다면
        if (CanvasUtility.IsDropMyField())
        {
            MoveCardFromDeckToField();
        }
    }


    // 원래 여기서 RPC로 호출하려고 했는데
    // 부모 클래스여서 뭔가 안되는 듯?
    // 자식 클래스로 옮기니까 잘됨
    protected abstract void MoveCardFromDeckToField();
}
