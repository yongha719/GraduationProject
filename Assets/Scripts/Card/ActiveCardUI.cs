using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActiveCardUI : MonoBehaviour
{
    private Button selectBtn;

    private Button informationBtn;

    private DeckBuildingCard card;

    private void Start()
    {
        
    }

    private void AddListner()
    {
        selectBtn.onClick.AddListener(() =>
        {

        });
    }
}
