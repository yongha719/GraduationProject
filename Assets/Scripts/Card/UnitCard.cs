using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Deck, // ���� ���� ��
    Field // �ʵ忡 ���� ��
}

public class UnitCard : MonoBehaviour, IUnitCard
{
    private int hp;

    [Tooltip("�ú����� ����")]
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
    [Tooltip("Ŭ�������� ���콺 �����Ϳ� ī�� �߾ӿ����� �Ÿ�")]
    Vector2 mousePosDistance;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        //printf
    }

    private void OnMouseEnter()
    {
        if (IsEnemy) return;

        // ī�尡 ���� ���� ��
        if (cardState == CardState.Deck)
        {
            // TODO ī��Ŀ���� �ϱ�
            return;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (IsEnemy) return;

        // ���� ��ư���� �巡�� ���������� ���� ������ ����� ���콺 �����Ϳ� �Ÿ��� ����
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (IsEnemy) return;

        // �巡���� �� ������ �ٲ���
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rect.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (IsEnemy) return;

        // �ٽ� ���ư�
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
                print("����");
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
}

interface IHandlers : IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

}