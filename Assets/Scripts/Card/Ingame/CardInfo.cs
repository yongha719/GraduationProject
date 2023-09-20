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
    [SerializeField]
    protected Sprite cardBackSprite;

    [SerializeField]
    protected Sprite deckCardSprite;

    [SerializeField]
    protected Sprite fieldCardSprite;

    [Space(15f), Header("Stats")]
    [SerializeField]
    protected GameObject deckStat;

    [SerializeField]
    protected GameObject fieldStat;

    [Space(15f), Header("Deck Stat Texts")]
    [SerializeField]
    protected TextMeshProUGUI deckHpText;

    [SerializeField]
    protected TextMeshProUGUI deckPowerText;

    [SerializeField]
    protected TextMeshProUGUI deckCostText;


    [Space(15f), Header("Field Stat Texts")]
    [SerializeField]
    protected TextMeshProUGUI fieldHpText;

    [SerializeField]
    protected TextMeshProUGUI fieldPowerText;

    private bool IsMine;

    private Image cardImageComponent;

    private Card card;

    private void Awake()
    {
        cardImageComponent = GetComponent<Image>();
    }

    public void Init(Card _card)
    {
        card = _card;
        
        IsMine = _card.IsMine;
        deckStat.SetActive(IsMine);

        switch (_card)
        {
            // 카드가 유닛카드와 마법카드가 있어서 이렇게 했음
            case UnitCard unitCard:
            {
                // 카드 이미지 불러오기
                var sprites = ResourceManager.GetUnitCardSprites(_card.name);

                deckCardSprite = sprites.deck;
                fieldCardSprite = sprites.field;

                unitCard.OnSetHpChange += hp => { fieldHpText.text = hp.ToString(); };

                unitCard.OnFieldChangeAction += OnFieldStateChange;

                // 텍스트 초기화
                deckHpText.text = $"{unitCard.CardData.Hp}";
                deckPowerText.text = $"{unitCard.CardData.Power}";
                deckCostText.text = $"{unitCard.CardData.Cost}";

                fieldHpText.text = deckHpText.text;
                fieldPowerText.text = deckPowerText.text;
                break;
            }
            case MasicCard masicCard:
            {
                print("Masic Card");
                var sprites = ResourceManager.GetMasicCardSprites(_card.name);
                deckCardSprite = sprites;

                print(deckStat.activeSelf);
                deckHpText.SetActive(false);
                deckPowerText.SetActive(false);

                deckCostText.text = masicCard.Cost.ToString();
                break;
            }
        }

        cardImageComponent.sprite = IsMine ? deckCardSprite : cardBackSprite;
    }

    public void OnFieldStateChange()
    {
        cardImageComponent.sprite = fieldCardSprite;

        deckStat.SetActive(false);
        fieldStat.SetActive(true);
    }
}