using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
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

    [SerializeField]
    private bool IsMine;

    [SerializeField, Tooltip("테스트 카드")]
    private GameObject Card;
    private string CardPath => $"Cards/{Card.name}";


    private void Start()
    {
        IsMine = photonView.ViewID == (int)PhotonViewType.PlayerDeck;
    }

    private void Update()
    {
        if (GameManager.Instance.IsTest && IsMine && Input.GetKeyDown(KeyCode.Alpha1))
            CardDraw();
    }

    public void CardDraw()
    {
        PhotonView cardPhotonView = PhotonNetwork.Instantiate(CardPath, Vector2.zero, Quaternion.identity).GetPhotonView();

        if (GameManager.Instance.IsTest)
            photonView.RPC(nameof(SetDeckParent), RpcTarget.AllBuffered, cardPhotonView.ViewID);
        else
            SetDeckParent(cardPhotonView.ViewID);
    }

    [PunRPC]
    private void SetDeckParent(int cardViewId)
    {
        PhotonView card = PhotonNetwork.GetPhotonView(cardViewId);
        PhotonView parentPhotonView = null;

        parentPhotonView = PhotonManager.GetPhotonViewByType(card.IsMine ? PhotonViewType.PlayerDeck : PhotonViewType.EnemyDeck);
        card.GetComponent<UnitCard>().AddPlayerUnit();

        card.gameObject.transform.SetParent(parentPhotonView.gameObject.transform);
        card.gameObject.transform.localScale = Vector3.one;
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
