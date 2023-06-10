using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public enum CardState
{
    Deck, // 덱에 있을 때
    ExpansionDeck, // 덱에 있는거 크게 볼때
    Field // 필드에 냈을 때
}

[Serializable]
[RequireComponent(typeof(CardDragAndDrop))]
/// <summary> 인게임 카드의 부모 클래스 </summary>
public abstract class Card : MonoBehaviourPun
{
    [SerializeField] protected bool IsEnemy;

    [SerializeField] protected CardState cardState = CardState.Deck;

    public virtual CardState CardState
    {
        get => cardState;
        set => cardState = value;
    }

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    protected bool CanDrag => cardDragAndDrop.CanDrag;

    /// <summary> 공격 가능한 상태인지 체크 </summary>
    public bool CanAttack => IsEnemy == false && CardState == CardState.Field && TurnManager.Instance.MyTurn;

    [Space(15f)] public CardData CardData;

    [Space(15f), Header("Sprites")] [SerializeField]
    protected Sprite originalCardSprite;

    [SerializeField] protected Sprite cardBackSprite;
    [SerializeField] protected Sprite fieldCardSprite;

    [Space(15f), Header("Stats")] [SerializeField]
    protected GameObject deckStat;

    [SerializeField] protected GameObject fieldStat;

    [Space(15f), Header("Deck Stat Texts")] [SerializeField]
    protected TextMeshProUGUI deckHpText;

    [SerializeField] protected TextMeshProUGUI deckPowerText;
    [SerializeField] protected TextMeshProUGUI deckCostText;


    [Space(15f), Header("Field Stat Texts")] [SerializeField]
    protected TextMeshProUGUI fieldHpText;

    [SerializeField] protected TextMeshProUGUI fieldPowerText;


    protected RectTransform rect;
    protected Image cardImageComponent;
    protected LineRenderer lineRenderer;

    protected CardDragAndDrop cardDragAndDrop;

    protected virtual void Awake()
    {
        // 카드 오브젝트의 이름은 카드의 등급으로 되어있기 때문에 이름을 카드 등급만 남게해줌
        name = name.Replace("(Clone)", "");

        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        CardManager.Instance.TryGetCardData(name, ref CardData);

        rect = transform as RectTransform;

        cardImageComponent = GetComponent<Image>();

        cardDragAndDrop = GetComponent<CardDragAndDrop>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;

        originalCardSprite = cardImageComponent.sprite;

        if (IsEnemy)
        {
            cardImageComponent.sprite = cardBackSprite;

            deckStat.SetActive(false);
        }

        cardDragAndDrop.OnEndDrag += OnEndDrag;
        cardDragAndDrop.OnDrop += OnDrop;

        deckHpText.text = CardData.Hp.ToString();
        deckPowerText.text = CardData.Power.ToString();
        deckCostText.text = CardData.Cost.ToString();

        fieldHpText.text = deckHpText.text;
        fieldPowerText.text = deckPowerText.text;
    }

    public void Init(Transform parent)
    {
        transform.SetParent(parent);
        transform.localScale = Vector3.one;

        // PosZ가 0이 아니라서 콜라이더 크기가 이상해짐
        // Vector2로 대입해줘서 0 만들어주기
        rect.anchoredPosition3D = rect.anchoredPosition;
    }

    protected virtual void OnEndDrag()
    {
        Attack();
    }

    protected virtual void OnDrop()
    {
        Attack();

        if (CanvasUtility.IsDropMyField())
            MoveCardFromDeckToField();
    }

    protected abstract void Attack();

    // 원래 여기서 RPC로 호출하려고 했는데
    // 부모 클래스여서 뭔가 안되는 듯?
    // 자식 클래스로 옮기니까 잘됨
    protected abstract void MoveCardFromDeckToField();

    public void Destroy()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PhotonManager.PhotonViewRemove(photonView.ViewID);
    }
}