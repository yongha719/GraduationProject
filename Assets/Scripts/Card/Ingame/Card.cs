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

public class Card : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPunObservable
{
    protected bool IsEnemy;
    protected bool CanDrag => IsEnemy == false && cardState == CardState.Deck;

    protected CardState cardState;
    public virtual CardState CardState { get; set; }

    private Vector2 originPos;
    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    private Vector2 mousePosDistance;

    protected RectTransform rect;


    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;
    }


    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    private void OnMouseEnter()
    {
        if (IsEnemy) return;

        // 카드가 덱에 있을 때
        if (cardState == CardState.Deck)
        {
            // TODO 카드커지게 하기
            return;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (CanDrag) return;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (CanDrag) return;

        // 드래그할 때 포지션 바꿔줌
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rect.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (CanDrag) return;

        // 다시 돌아가
        rect.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (CanDrag) return;

        var rayhits = Physics2D.RaycastAll(transform.position + Vector3.back, Vector3.forward, 10f);

        for (int i = 0; i < rayhits.Length; i++)
        {
            if (rayhits[i].collider.TryGetComponent(out UnitCard card) && card.IsEnemy)
            {
                print("드디어");
            }
        }
    }

}
