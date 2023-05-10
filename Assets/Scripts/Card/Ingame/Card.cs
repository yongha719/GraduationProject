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
public class Card : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public CardData CardData;

    protected bool IsEnemy;

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    protected bool CanDrag => IsEnemy == false && CardState == CardState.Deck && TurnManager.Instance.MyTurn;

    protected CardState cardState = CardState.Deck;
    public virtual CardState CardState
    {
        get => cardState;

        set => photonView.RPC(nameof(SetCardStateRPC), RpcTarget.AllBuffered, value);
    }


    protected RectTransform rect;
    protected LineRenderer lineRenderer;

    private Vector2 originPos;
    private Quaternion layoutRot;
    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;

        print(nameof(Card));
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

    [PunRPC]
    private void SetCardStateRPC(CardState value)
    {
        switch (value)
        {
            case CardState.Deck:
                rect.localScale = Vector3.one;
                lineRenderer.positionCount = 0;
                break;
            case CardState.ExpansionDeck:
                // 덱에 있는 카드를 눌렀을 때 커지는 모션
                break;
            case CardState.Field:
                rect.localScale = Vector3.one * 0.6f;
                lineRenderer.positionCount = 2;
                break;
        }

        CardState = value;
    }

    protected virtual void Attack(UnitCard card)
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
        if (CanDrag == false) return;

        // 다시 돌아가
        transform.localRotation = layoutRot;
        rect.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (cardState == CardState.Field)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
            }

            var rayhits = Physics2D.RaycastAll(transform.position + Vector3.back, Vector3.forward, 10f);

            for (int i = 0; i < rayhits.Length; i++)
            {
                // 임시 공격
                if (rayhits[i].collider.TryGetComponent(out UnitCard card) && card.IsEnemy)
                {
                    Attack(card);
                }
            }
        }

        if (CanDrag == false) return;

        // 필드에 드롭된다면
        if (CanvasUtility.IsDropMyField())
        {
            MoveCardFromDeckToField();
            return;
        }
    }

    public bool CanAttack()
    {
        if (IsEnemy && cardState == CardState.Field && CardManager.Instance.EnemyHasTauntCard((UnitCard)this))
            return true;
        else
            return false;
    }

    // 원래 여기서 RPC로 호출하려고 했는데
    // 부모 클래스여서 뭔가 안되는 듯?
    // 자식 클래스로 옮기니까 잘됨
    protected virtual void MoveCardFromDeckToField()
    {
    }
}
