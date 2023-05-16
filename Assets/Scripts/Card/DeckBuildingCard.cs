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
    #endregion

    [ReadOnlyAttributeA]
    private DragCard currentDranggingCard;

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
        currentDranggingCard = DeckManager.Instance.SpawnDragCardTemp(transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentDranggingCard.transform.position = eventData.position;
        if (currentDranggingCard.rect.anchoredPosition.x > cardChangeStandardXPos)
        {
            currentDranggingCard.IsSelectPosition = true;
        }
        else
        {
            currentDranggingCard.IsSelectPosition = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentDranggingCard.IsSelectPosition == true)
        {
            DeckManager.Instance.SelectCard(data);
        }
        else
        {

        }
        Destroy(currentDranggingCard.gameObject);
    }
}
