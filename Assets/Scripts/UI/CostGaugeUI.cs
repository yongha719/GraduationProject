using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostGaugeUI : MonoBehaviour
{
    private Sprite costGaugeBottomSprite;
    private Sprite costGaugeMiddleSprite;
    private Sprite costGaugeTopSprite;

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.Instance.OnPlayerTurnAction += () =>
        {
            print("Cost Ui");

            CostGaugeChange(GameManager.Instance.Cost);
        };
    }

    private void CostGaugeChange(uint cost)
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}