using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Deck, // 덱에 있을 때
    Field // 필드에 냈을 때
}

public class UnitCard : MonoBehaviourPun, IUnitCard, IPunObservable
{
    private int hp;

    [Tooltip("시봉봉의 억지")]
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

    public CardData CardData;
    public bool IsEnemy;

    
    Vector2 originPos;
    [Tooltip("클릭했을때 마우스 포인터와 카드 중앙에서의 거리")]
    Vector2 mousePosDistance;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        print(rect == null);
    }

    protected virtual void Start()
    {
        
    }

    [PunRPC]
    private void setParentAndViewID(int viewID)
    {
        print("RPC");
        PhotonView parentView = PhotonView.Find(viewID);
        transform.SetParent(parentView.gameObject.transform);
        transform.localScale = Vector3.one;
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
        if (IsEnemy) return;

        // 왼쪽 버튼으로 드래그 시작했을때 원래 포지션 저장과 마우스 포인터와 거리도 저장
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (IsEnemy) return;

        // 드래그할 때 포지션 바꿔줌
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rect.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (IsEnemy) return;

        // 다시 돌아가
        rect.anchoredPosition = originPos;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (IsEnemy) return;

        var rayhits = Physics2D.RaycastAll(transform.position + Vector3.back, Vector3.forward, 10f);

        print(transform.position);

        for (int i = 0; i < rayhits.Length; i++)
        {
            if (rayhits[i].collider.TryGetComponent(out UnitCard card) && card.IsEnemy)
            {
                print("드디어");
            }
        }
    }

    void IUnitCard.Heal(int healhp)
    {
        throw new System.NotImplementedException();
    }

    void IUnitCard.Hit(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }
}

interface IHandlers : IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

}