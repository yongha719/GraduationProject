using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnChangeButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void TurnChange()
    {
        if (TurnManager.Instance.MyTurn)
            TurnManager.Instance.TurnChange();
    }
}
