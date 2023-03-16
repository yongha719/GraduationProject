using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDeck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rect;

    [SerializeField]
    private Image illust;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI abilityExplaneText;
    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField]
    private TextMeshProUGUI power;
    [SerializeField]
    private TextMeshProUGUI hp;

    [SerializeField]
    private CardData cardData;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void SetCardDeck()
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
