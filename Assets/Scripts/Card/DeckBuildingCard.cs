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
public class DeckBuildingCard : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rect;

    public CardData data;

    #region UI
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

    [SerializeField]
    private TextMeshProUGUI cardCount;
    #endregion

    [ReadOnlyAttributeA]
    private DragCard currentDraggingCard;

    public DragCard dragObj;

    public Image img;

    [SerializeField]
    [Tooltip("선택된 상태의 오브젝트")]
    private Image selectObj;

    [SerializeField]
    [Tooltip("선택되지 않은 상태의 오브젝트")]
    private Image deSelectObj;

    [Tooltip("카드가 바뀌는 기준X좌표")]
    public float cardChangeStandardXPos;

    [SerializeField]
    private bool isSelect;
    public bool IsSelect
    {
        get => isSelect;
        set
        {
            isSelect = value;

            ChangeCard(isSelect);
        }
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();
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

    private void ChangeCard(bool isSelect)
    {
        selectObj.gameObject.SetActive(isSelect);
        deSelectObj.gameObject.SetActive(!isSelect);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentDraggingCard = DeckManager.Instance.SpawnDragCardTemp(transform.position);
        currentDraggingCard.IsSelectPosition = isSelect;
        currentDraggingCard.isOriginSelection = isSelect;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentDraggingCard.transform.position = eventData.position;
        if (currentDraggingCard.rect.anchoredPosition.x > cardChangeStandardXPos)
        {
            currentDraggingCard.IsSelectPosition = true;
        }
        else
        {
            currentDraggingCard.IsSelectPosition = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentDraggingCard.IsSelectPosition == true && currentDraggingCard.isOriginSelection == false)
        {
            DeckManager.Instance.SelectCard(data);
        }
        else if(currentDraggingCard.IsSelectPosition == false && currentDraggingCard.isOriginSelection == true)
        {
            DeckManager.Instance.DeSelectCard(this);
        }

        Destroy(currentDraggingCard.gameObject);
    }
}
