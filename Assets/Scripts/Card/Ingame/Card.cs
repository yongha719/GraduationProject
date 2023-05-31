using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using Unity.VisualScripting;

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
    [SerializeField]
    protected bool IsEnemy;

    [SerializeField]
    protected CardState cardState = CardState.Deck;
    public virtual CardState CardState { get => cardState; set => cardState = value; }

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    protected bool CanDrag => cardDragAndDrop.CanDrag;

    /// <summary> 공격 가능한 상태인지 체크 </summary>
    public bool CanAttack => IsEnemy == false && CardState == CardState.Field && TurnManager.Instance.MyTurn;


    [SerializeField] protected TextMeshProUGUI hpText;
    [SerializeField] protected TextMeshProUGUI powerText;
    [SerializeField] protected TextMeshProUGUI costText;

    protected RectTransform rect;
    protected LineRenderer lineRenderer;

    public CardData CardData;
    protected CardDragAndDrop cardDragAndDrop;

    protected virtual void Awake()
    {
        // 카드 오브젝트의 이름은 카드의 등급으로 되어있기 때문에 이름을 카드 등급만 남게해줌
        name = name.Replace("(Clone)", "");

        // 오브젝트의 이름이 카드의 등급이고 딕셔너리의 키 값이 카드의 등급임 
        print(name);
        CardManager.Instance.TryGetCardData(name, ref CardData);

        rect = transform as RectTransform;
        lineRenderer = GetComponent<LineRenderer>();

        cardDragAndDrop = GetComponent<CardDragAndDrop>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;

        cardDragAndDrop.OnEndDrag += OnEndDrag;
        cardDragAndDrop.OnDrop += OnDrop;

        hpText.text = CardData.Hp.ToString();
        powerText.text = CardData.Power.ToString();
        costText.text = CardData.Cost.ToString();
    }

    public void Init(Transform parent)
    {
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
    }

    protected virtual void OnEndDrag()
    {
        Attack();
    }

    protected virtual void OnDrop()
    {
        print("onDrop");
        
        Attack();

        if (CanvasUtility.IsDropMyField())
            MoveCardFromDeckToField();
    }

    protected abstract void Attack();

    // 원래 여기서 RPC로 호출하려고 했는데
    // 부모 클래스여서 뭔가 안되는 듯?
    // 자식 클래스로 옮기니까 잘됨
    protected abstract void MoveCardFromDeckToField();

    private void OnDestroy()
    {
        PhotonManager.PhotonViewRemove(photonView.ViewID);
    }
}
