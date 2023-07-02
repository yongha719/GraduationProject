using System;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>인게임에서 덱에 있는 카드의 레이아웃 </summary>
[AddComponentMenu("MyComponent/Card Deck Layout", int.MinValue)]
public class CardDeckLayout : MonoBehaviourPunCallbacks, IPunObservable
{
    private readonly Vector3 leftPosition = new Vector3(-200, -30, 0);
    private readonly Vector3 rightPosition = new Vector3(420, -30, 0);
    private readonly Quaternion leftRotation = Quaternion.Euler(0, 0, 15f);
    private readonly Quaternion rightRotation = Quaternion.Euler(0, 0, -15f);

    [SerializeField] private SerializedDictionary<string, GameObject> photonResources = new();

    [SerializeField] private bool IsMine;


    /// <summary> 인게임 카드 경로 </summary>
    /// 포톤에서 Resource.Load를 사용하기 때문에
    /// 이런식으로 경로를 정의함
    private string CardPath => "Cards/In Game Card";

    private void Start()
    {
        IsMine = photonView.ViewID == (int)PhotonViewType.PlayerDeck;

        if (IsMine)
            CardManager.Instance.CardDraw += () => CardDraw();
        else
            CardManager.Instance.EnemyCardDraw += () => CardDraw();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (IsMine && Input.GetKeyDown(KeyCode.Alpha1))
            CardDraw();
#endif
    }

    public void CardDraw(int count, bool istest = false)
    {
        for (int i = 0; i < count; i++)
            CardDraw(istest);
    }

    // 테스트 카드 소환
    public void CardDraw(bool isTest = false)
    {
        var cardObj = PhotonNetwork.Instantiate(CardPath, Vector2.zero, Quaternion.identity);

        PhotonView cardPhotonView = cardObj.GetPhotonView();

        string cardName = CardManager.Instance.GetRandomCardName();

        if (isTest)
            SetCardAndParentRPC(cardPhotonView.ViewID, cardName);
        else
            photonView.RPC(nameof(SetCardAndParentRPC), RpcTarget.AllBuffered,
                cardPhotonView.ViewID, cardName);
    }


    /// <summary>
    /// 카드 이름과 필드 세팅
    /// </summary>
    /// RPC 호출이라 다른 개체에서는 어떤 개체인지 모르기 때문에
    /// viewId를 모르기 때문에 인자로 넘겨줘야함
    [PunRPC]
    private void SetCardAndParentRPC(int cardViewId, string cardName)
    {
        PhotonView cardPhotonView = PhotonManager.GetPhotonView(cardViewId);

        PhotonView parentPhotonView =
            PhotonManager.GetPhotonViewByType(cardPhotonView.IsMine
                ? PhotonViewType.PlayerDeck
                : PhotonViewType.EnemyDeck);

        cardPhotonView.gameObject.name = cardName;

        var cardType = cardPhotonView.gameObject.AddComponent(Type.GetType($"{cardName}Unit"));

        if (cardType is Card card)
            card.Init(parentPhotonView.transform, cardName);
    }

    private void OnTransformChildrenChanged()
    {
        float[] lerpValue;

        switch (transform.childCount)
        {
            case 1:
                lerpValue = new float[] { 0.5f };
                break;
            case 2:
                lerpValue = new float[] { 0.27f, 0.73f };
                break;
            case 3:
                lerpValue = new float[] { 0.1f, 0.5f, 0.9f };
                break;
            default:
                lerpValue = new float[transform.childCount];

                float interval = 1f / (transform.childCount - 1);

                for (int i = 0; i < transform.childCount; i++)
                    lerpValue[i] = interval * i;
                break;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 targetPos = Vector3.Lerp(leftPosition, rightPosition, lerpValue[i]);
            Quaternion targetRos = Quaternion.identity;

            if (transform.childCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(0.5f, 2f) - Mathf.Pow(lerpValue[i] - 0.5f, 2)) * 80f;
                targetPos.y += curve;
                targetRos = Quaternion.Slerp(leftRotation, rightRotation, lerpValue[i]);
            }

            RectTransform rect = transform.GetChild(i) as RectTransform;
            rect.anchoredPosition = targetPos;
            rect.localRotation = targetRos;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}