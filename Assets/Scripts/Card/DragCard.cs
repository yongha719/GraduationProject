using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragCard : MonoBehaviour
{
    private CardData data;

    public GameObject selectCardObj;

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

    private void Start()
    {
        StartCoroutine(IUpdate());
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
