using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

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
    protected bool IsEnemy;

    protected CardState cardState = CardState.Deck;
    public virtual CardState CardState { get; set; }


    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    protected bool CanDrag => cardDragAndDrop.CanDrag;

    /// <summary> 공격 가능한 상태인지 체크 </summary>
    public bool CanAttack => IsEnemy == false && CardState == CardState.Field && TurnManager.Instance.MyTurn;


    protected RectTransform rect;
    protected LineRenderer lineRenderer;

    public CardData CardData;
    protected CardDragAndDrop cardDragAndDrop;

    protected virtual void Awake()
    {
        // 카드 오브젝트의 이름은 카드의 등급으로 되어있기 때문에 이름을 카드 등급만 남게해줌
        name = name.Replace("(Clone)", "");

        rect = transform as RectTransform;
        lineRenderer = GetComponent<LineRenderer>();


        cardDragAndDrop = GetComponent<CardDragAndDrop>();
    }

    protected virtual void Start()
    {
        IsEnemy = !photonView.IsMine;

        cardDragAndDrop.OnDrop = OnDrop;
    }

    private void OnMouseEnter()
    {
        if (CanDrag == false) return;

        // 카드가 덱에 있을 때 카드에 마우스 올리면
        if (cardState == CardState.Deck)
        {
            // TODO 카드커지게 하기
            return;
        }
    }

    protected void OnDrop()
    {
        Attack();

        if (CanDrag == false) return;

        if (CanvasUtility.IsDropMyField())
            MoveCardFromDeckToField();

    }

    protected abstract void Attack();

    // 원래 여기서 RPC로 호출하려고 했는데
    // 부모 클래스여서 뭔가 안되는 듯?
    // 자식 클래스로 옮기니까 잘됨
    protected abstract void MoveCardFromDeckToField();
}
