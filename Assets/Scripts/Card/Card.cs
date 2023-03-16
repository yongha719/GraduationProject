using UnityEngine;
using UnityEngine.EventSystems;

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

    public CardData cardData;
    public bool IsEnemy;

    Vector2 originPos;
    // Ŭ�������� ���콺 �����Ϳ� ī�� �߾ӿ����� �Ÿ�
    Vector2 mousePosDistance;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originPos = rect.anchoredPosition;
        mousePosDistance = originPos - CanvasUtility.GetMousePosToCanvasPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = CanvasUtility.GetMousePosToCanvasPos();

        if (Mathf.Abs(originPos.x - mousePos.x) >= 50f && Mathf.Abs(originPos.y - mousePos.y) >= 50f)
        {
            rect.anchoredPosition = mousePosDistance + mousePos;
            print("ddd");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
