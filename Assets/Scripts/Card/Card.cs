using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Deck, Field
}

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private int hp;
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

            if(cardState == CardState.Deck)
            {
                rect.localScale = Vector3.one;
            }
            else if (cardState == CardState.Field)
            {
                rect.localScale = Vector3.one * 0.6f;
            }
        }
    }

    public CardData cardData;
    public bool IsEnemy;

    Vector2 originPos;
    /// <summary>Ŭ�������� ���콺 �����Ϳ� ī�� �߾ӿ����� �Ÿ�</summary>
    Vector2 mousePosDistance;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���� ��ư���� �巡�� ���������� ���� ������ ����� ���콺 �����Ϳ� �Ÿ��� ����
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originPos = rect.anchoredPosition;
            mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡���� �� ������ �ٲ���
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

            rect.anchoredPosition = mousePosDistance + mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �ٽ� ���ư�
        rect.anchoredPosition = originPos;
    }

    private void OnMouseEnter()
    {
        // ī�尡 ���� ���� ��
        if (cardState == CardState.Deck)
        {
            // TODO ī��Ŀ���� �ϱ�
        }
    }

    private void OnMouseExit()
    {

    }


}
