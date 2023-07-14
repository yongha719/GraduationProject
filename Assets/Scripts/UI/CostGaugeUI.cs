using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class CostGaugeUI : MonoBehaviourPun
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
        CostGaugeChangeRPC();

        GameManager.Instance.CostGaugeUI = this;

        TurnManager.Instance.OnTurnChangeAction += () =>
        {
            CostGaugeChange();
        };
    }

    public void CostGaugeChange()
    {
        if (PhotonManager.IsAlone)
            CostGaugeChangeRPC();
        else
            photonView.RPC(nameof(CostGaugeChangeRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void CostGaugeChangeRPC()
    {
        print("Cost Change");

        var maxCost = GameManager.Instance.MaxCost;
        var cost = GameManager.Instance.Cost;

        for (int i = 0; i < costGaugeBackgroundParts.Count; i++)
        {
            costGaugeBackgroundParts[i].SetActive(i < maxCost - 1);
            costGaugeParts[i].SetActive(i < cost);
        }

        costGaugeBackgroundParts[9].SetActive(true);
        if (cost != 0)
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