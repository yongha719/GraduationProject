using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;


/// <summary>인게임에서 덱에 있는 카드의 레이아웃 </summary>
[AddComponentMenu("MyComponent/CardLayout", int.MinValue)]
public class CardDeckLayout : MonoBehaviourPunCallbacks, IPunObservable
{
    private readonly Vector3 leftPosition = new Vector3(-200, -30, 0);
    private readonly Vector3 rightPosition = new Vector3(420, -30, 0);
    private readonly Quaternion leftRotation = Quaternion.Euler(0, 0, 15f);
    private readonly Quaternion rightRotation = Quaternion.Euler(0, 0, -15f);

    private List<RectTransform> childs = new List<RectTransform>();

    [SerializeField] private SerializedDictionary<string, GameObject> photonResources = new();

    [SerializeField] private bool IsMine;

    /// <summary> 아직 테스트 중이라 이런식으로 함 </summary>
    private string testCardPath => $"Cards/In game Cards/{CardManager.Instance.GetRandomCardName()}";

    private const string CardPath = "Cards/In game Cards/In Game Card";

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

    // 테스트 카드 소환
    public void CardDraw(bool isTest = false)
    {
        var card = CardManager.Instance.GetRandomCardGameObject();

        PhotonView cardPhotonView =
            PhotonNetwork.Instantiate(testCardPath, Vector2.zero, Quaternion.identity).GetPhotonView();
        // PhotonView cardPhotonView = PhotonNetwork.Instantiate(card, Vector2.zero, Quaternion.identity).GetPhotonView();

        photonResources = PhotonNetwork.GetResources();

        if (isTest)
            SetDeckParentRPC(cardPhotonView.ViewID);
        else
            photonView.RPC(nameof(SetDeckParentRPC), RpcTarget.AllBuffered, cardPhotonView.ViewID);
    }

    /// <summary>
    /// 덱에서 필드로 낸 카드의 
    /// </summary>
    /// RPC 호출이라 다른 개체에서는 어떤 개체인지 모르기 때문에
    /// viewId를 모르기 때문에 인자로 넘겨줘야함
    [PunRPC]
    private void SetDeckParentRPC(int cardViewId)
    {
        PhotonView cardPhotonView = PhotonManager.TryGetPhotonView(cardViewId);

        PhotonView parentPhotonView =
            PhotonManager.GetPhotonViewByType(cardPhotonView.IsMine
                ? PhotonViewType.PlayerDeck
                : PhotonViewType.EnemyDeck);

        cardPhotonView.GetComponent<Card>().Init(parentPhotonView.gameObject.transform);
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