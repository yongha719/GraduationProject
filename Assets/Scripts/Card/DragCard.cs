using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragCard : MonoBehaviour,IPointerUpHandler, IDragHandler
{
    private CardData data;

    public GameObject selectCardObj;

    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField]
    private TextMeshProUGUI powerText;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI explainText;

    public void SetDragCard(CardData data)
    {
        this.data = data;
        costText.text = $"{data.Cost}";
        powerText.text = $"{data.Damage}";
        hpText.text = $"{data.Hp}";
        nameText.text = $"{data.Name}";
        explainText.text = $"{data.BasicAttackExplain}";
    }

    private void Update()
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
