using System;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class CardInfo : MonoBehaviourPun
{
    public CardData CardData;
    
    [Space(15f), Header("Sprites")]

    [SerializeField] protected Sprite cardBackSprite;
    [SerializeField] protected Sprite deckCardSprite;
    [SerializeField] protected Sprite fieldCardSprite;
    
    [Space(15f), Header("Stats")]

    [SerializeField] protected GameObject deckStat;
    [SerializeField] protected GameObject fieldStat;

    [Space(15f), Header("Deck Stat Texts")]

    [SerializeField] protected TextMeshProUGUI deckHpText;
    [SerializeField] protected TextMeshProUGUI deckPowerText;
    [SerializeField] protected TextMeshProUGUI deckCostText;


    [Space(15f), Header("Field Stat Texts")]

    [SerializeField] protected TextMeshProUGUI fieldHpText;
    [SerializeField] protected TextMeshProUGUI fieldPowerText;

    private void Start()
    {
        var sprites = ResourceManager.Instance.GetCardSprites(name);

        deckCardSprite = sprites.Item1;
        fieldCardSprite = sprites.Item2;
        
        deckHpText.text = CardData.Hp.ToString();
        deckPowerText.text = CardData.Power.ToString();
        deckCostText.text = CardData.Cost.ToString();

        fieldHpText.text = deckHpText.text;
        fieldPowerText.text = deckPowerText.text;
    }

    public void SetName(string name)
    {
        gameObject.name = name;
        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        CardManager.Instance.TryGetCardData(name, ref CardData);
    }
}
