using System;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


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


    private bool IsEnemy;

    private Image cardImageComponent;

    private Card card;

    private void Awake()
    {
        cardImageComponent = GetComponent<Image>();

        card = GetComponent<Card>();
    }

    private void Start()
    {
        IsEnemy = !photonView.IsMine;

        #region Init Texts

        deckHpText.text = CardData.Hp.ToString();
        deckPowerText.text = CardData.Power.ToString();
        deckCostText.text = CardData.Cost.ToString();

        fieldHpText.text = deckHpText.text;
        fieldPowerText.text = deckPowerText.text;

        #endregion

        // 카드가 유닛카드와 마법카드가 있어서 이렇게 했음
        if (card is UnitCard unitCard)
            unitCard.OnSetHpChange += (hp) => fieldHpText.text = hp.ToString();
    }

    public void Init(string name)
    {
        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        CardManager.Instance.TryGetCardData(name, ref CardData);

        gameObject.name = name + (IsEnemy ? "_Enemy" : "_Player");

        var sprites = ResourceManager.GetCardSprites(name);

        deckCardSprite = sprites.deck;
        fieldCardSprite = sprites.field;

        cardImageComponent.sprite = IsEnemy ? cardBackSprite : deckCardSprite;

        deckStat.SetActive(!IsEnemy);
    }

    public void OnFieldStateChange()
    {
        cardImageComponent.sprite = fieldCardSprite;

        deckStat.SetActive(false);
        fieldStat.SetActive(true);
    }
}
