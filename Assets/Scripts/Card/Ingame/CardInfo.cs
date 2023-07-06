using System;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CardInfo : MonoBehaviourPun
{
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
    }

    private void Start()
    {
        #region Init Texts

        if (card is UnitCard unitcard)
        {
            deckHpText.text = unitcard.CardData.Hp.ToString();
            deckPowerText.text = unitcard.CardData.Power.ToString();
            deckCostText.text = unitcard.CardData.Cost.ToString();

            fieldHpText.text = deckHpText.text;
            fieldPowerText.text = deckPowerText.text;
        }
        else if(card is MasicCard masicCard)
        {

        }
        #endregion
    }

    public void Init(Card card, string name)
    {
        this.card = card;

        // 카드가 유닛카드와 마법카드가 있어서 이렇게 했음
        if (card is UnitCard unitCard)
        {
            print("card is Unitcard");
            unitCard.OnSetHpChange += hp =>
            {
                print("hp Change");
                fieldHpText.text = hp.ToString();
            };
        }

        else if (card is MasicCard masicCard)
        {
            deckHpText.gameObject.SetActive(false);
            deckPowerText.gameObject.SetActive(false);
        }

        print("card info init");

        IsEnemy = !photonView.IsMine;

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
