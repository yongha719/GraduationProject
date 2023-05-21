using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class DragCard : MonoBehaviour
{
    private CardData data;

    [HideInInspector]
    public RectTransform rect;

    [Tooltip("선택된 상태의 오브젝트")]
    public GameObject selectCardObj;

    [Tooltip("선택되지 않은 상태의 오브젝트")]
    public GameObject deSelectCardObj;

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

    public float standardX;

    private bool isSelectPosition;
    public bool IsSelectPosition
    {
        get => isSelectPosition;
        set
        {
            isSelectPosition = value;

            selectCardObj.SetActive(value);
            deSelectCardObj.SetActive(!value);
        }
    }

    public bool isOriginSelection;

    private void Start()
    {
        StartCoroutine(IUpdate());
        rect = GetComponent<RectTransform>();
    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            print(transform.position);
        }
    }

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
}
