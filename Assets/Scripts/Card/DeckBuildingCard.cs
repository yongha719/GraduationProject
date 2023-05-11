using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// 덱 편성하는 카드
/// </summary>
public class DeckBuildingCard : MonoBehaviour, IPointerDownHandler
{
    //public CardData data;

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

    void Start()
    {

    }

    private void SetCard(CardData data)
    {
        //this.data = data;
        costText.text = $"{data.Cost}";
        powerText.text = $"{data.Power}";
        hpText.text = $"{data.Hp}";
        nameText.text = $"{data.Name}";
        explainText.text = $"{data.BasicAttackExplain}";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DeckManager.Instance.SpawnDragCardTemp(transform.position);
        //DeckManager.Instance.SpawnDragCard(data);
    }
}
