using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CostGaugeUI : MonoBehaviour
{
    private Sprite costGaugeBottomSprite;
    private Sprite costGaugeMiddleSprite;
    private Sprite costGaugeTopSprite;

    [Tooltip("코스트 게이지칸"), SerializeField] 
    private List<GameObject> costGaugeParts = new(10);

    private List<GameObject> curCostGaugeParts = new(10);

    [Tooltip("코스트 게이지칸 배경"), SerializeField]
    private List<GameObject> costGaugeBackgroundParts = new(10);

    void Start()
    {
        // Test
        CostGaugeChange();

        TurnManager.Instance.OnPlayerTurnAction += () =>
        {
            print("Cost UI");

            CostGaugeChange();
        };
    }

    private void CostGaugeChange()
    {
        curCostGaugeParts.Clear();

        for (var i = 0; i < costGaugeBackgroundParts.Count; i++)
        {
            costGaugeBackgroundParts[i].SetActive(false);
            costGaugeParts[i].SetActive(false);
        }

        var maxCost = GameManager.Instance.MaxCost;

        if (maxCost > 0)
        {
            for (int i = 0; i < maxCost - 1; i++)
            {
                costGaugeParts[i].SetActive(true);
                curCostGaugeParts.Add(costGaugeParts[i]);

                costGaugeBackgroundParts[i].SetActive(true);
            }

            costGaugeParts[9].SetActive(true);
            curCostGaugeParts.Add(costGaugeParts[9]);

            costGaugeBackgroundParts[9].SetActive(true);
        }

        foreach (var costGaugePart in curCostGaugeParts)
            costGaugePart.SetActive(false);

        for (int i = 0; i < GameManager.Instance.Cost; i++)
            curCostGaugeParts[i].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}