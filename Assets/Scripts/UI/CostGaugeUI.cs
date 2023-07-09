using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class CostGaugeUI : MonoBehaviour
{
    private Sprite costGaugeBottomSprite;
    private Sprite costGaugeMiddleSprite;
    private Sprite costGaugeTopSprite;

    [Space]

    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField] 
    private TextMeshProUGUI enemyCostText;
    

    [Space]

    [Tooltip("코스트 게이지칸"), SerializeField]
    private List<GameObject> costGaugeParts = new(10);


    [Tooltip("코스트 게이지칸 배경"), SerializeField]
    private List<GameObject> costGaugeBackgroundParts = new(10);


    void Start()
    {
        // Test
        CostGaugeChange();

        TurnManager.Instance.OnTurnChangeAction += () =>
        {
            print("Cost UI");

            CostGaugeChange();
        };
    }

    [PunRPC]
    public void CostGaugeChange()
    {
        var maxCost = GameManager.Instance.MaxCost;
        var cost = GameManager.Instance.Cost;

        for (int i = 0; i < costGaugeBackgroundParts.Count; i++)
        {
            costGaugeBackgroundParts[i].SetActive(i < maxCost - 1);
            costGaugeParts[i].SetActive(i < cost);
        }

        costGaugeBackgroundParts[9].SetActive(true);

        costGaugeParts[(int)cost - 1].SetActive(maxCost != cost);
        costGaugeParts[9].SetActive(maxCost == cost);

        costText.text = $"{cost}/{maxCost}";
        enemyCostText.text = $"{GameManager.Instance.EnemyCost}/{GameManager.Instance.EnemyMaxCost}";
    }

    // Update is called once per frame
    void Update()
    {
    }
}