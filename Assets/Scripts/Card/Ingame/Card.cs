using Photon.Pun;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public enum CardState
{
    Deck, // 덱에 있을 때
    ExpansionDeck, // 덱에 있는거 크게 볼때
    Field // 필드에 냈을 때
}

[Serializable]
/// <summary> 인게임 카드의 부모 클래스 </summary>
public abstract class Card : MonoBehaviourPun, IPunObservable
{
    public bool IsEnemy;

    [SerializeField] protected CardState cardState = CardState.Deck;

    public virtual CardState CardState
    {
        get => cardState;
        set => cardState = value;
    }

    /// <summary> 드래그 가능한 상태인지 체크 </summary>
    protected bool CanDrag => cardDragAndDrop.CanDrag;

    public virtual int Cost { get; }

    public RectTransform rect;
    protected Image cardImageComponent;

    protected CardInfo cardInfo;
    protected CardDragAndDrop cardDragAndDrop;

    protected virtual void Awake()
    {
        rect = transform as RectTransform;

        cardImageComponent = GetComponent<Image>();

        cardDragAndDrop = GetComponent<CardDragAndDrop>();
        cardInfo = GetComponent<CardInfo>();

        cardDragAndDrop.Init();

        IsEnemy = !photonView.IsMine;
    }

    protected virtual void Start()
    {
        cardDragAndDrop.OnEndDrag += OnEndDrag;
        cardDragAndDrop.OnDrop += OnDrop;
    }

    public void Init(Transform parent = null)
    {
        transform.localScale = Vector3.one;

        // PosZ가 0이 아니라서 콜라이더 크기가 이상해짐
        // Vector2로 대입해줘서 0 만들어주기
        rect.anchoredPosition3D = rect.anchoredPosition;

        print($"Card : {name}");
        cardInfo.Init(this);
    }

    protected virtual void OnEndDrag()
    {
        Attack();
    }

    protected virtual void OnDrop()
    {
        Attack();
    }

    protected virtual void Attack() { }

    public void Destroy()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        PhotonManager.PhotonViewRemove(photonView.ViewID);
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public static explicit operator Card(Type type)
    {
        // type이 Card의 자식 타입인지 확인함
        if (type != null && type.IsSubclassOf(typeof(Card)))
        {
            Card card = (Card)Activator.CreateInstance(type);
            return card;
        }
        else
        {
            throw new InvalidCastException($"{type}을(를) Card로 변환할 수 없습니다.");
        }
    }

}
