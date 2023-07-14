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

    public void Init(Card card)
    {
        IsEnemy = !photonView.IsMine;
        deckStat.SetActive(!IsEnemy);


        this.card = card;


        // 카드 이미지 불러오기
        var sprites = ResourceManager.GetUnitCardSprites(card.name);

        deckCardSprite = sprites.deck;

        cardImageComponent.sprite = IsEnemy ? cardBackSprite : deckCardSprite;


        // 카드가 유닛카드와 마법카드가 있어서 이렇게 했음
        if (card is UnitCard unitCard)
        {
            fieldCardSprite = sprites.field;

            unitCard.OnSetHpChange += hp =>
            {
                fieldHpText.text = hp.ToString();
            };

            // 텍스트 초기화
            deckHpText.text = unitCard.CardData.Hp.ToString();
            deckPowerText.text = unitCard.CardData.Power.ToString();
            deckCostText.text = unitCard.CardData.Cost.ToString();

            fieldHpText.text = deckHpText.text;
            fieldPowerText.text = deckPowerText.text;
        }
        else if (card is MasicCard masicCard)
        {
            deckHpText.gameObject.SetActive(false);
            deckPowerText.gameObject.SetActive(false);
        }

        gameObject.name = card.name + (IsEnemy ? "_Enemy" : "_Player");
    }

    public void OnFieldStateChange()
    {
        cardImageComponent.sprite = fieldCardSprite;

        deckStat.SetActive(false);
        fieldStat.SetActive(true);
    }
}
