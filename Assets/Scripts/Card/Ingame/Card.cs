using Photon.Pun;
using UnityEngine;
using System;
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
/// <summary> 인게임 카드의 부모 클래스 </summary>
public abstract class Card : MonoBehaviourPun, IPunObservable
{
    public bool IsMine;

    [SerializeField]
    protected CardState cardState = CardState.Deck;

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
    }

    protected virtual void Start()
    {
        gameObject.name = name + (IsMine ? "_Player" : "_Enemy");

        cardDragAndDrop.OnEndDrag += OnEndDrag;
        cardDragAndDrop.OnDrop += OnDrop;
    }

    public void Init(bool isMine, Transform parent = null)
    {
        IsMine = isMine;

        transform.localScale = Vector3.one;

        // PosZ가 0이 아니라서 콜라이더 크기가 이상해짐
        // Vector2로 대입해줘서 0 만들어주기
        rect.anchoredPosition3D = rect.anchoredPosition;

        cardInfo.Init(this);
        cardDragAndDrop.Init();
    }

    protected virtual void OnEndDrag()
    {
        DropField();
    }

    protected virtual void OnDrop()
    {
        DropField();
    }

    protected virtual void DropField()
    {
    }

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
}