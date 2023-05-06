using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardFieldLayout : MonoBehaviourPun
{
    [SerializeField]
    private bool IsMine;

    public List<UnitCard> Cards = new List<UnitCard>();

    private void Start()
    {
        IsMine = photonView.ViewID == (int)PhotonViewType.PlayerField;

        if (IsMine)
            CanvasUtility.MyFieldRect = (transform as RectTransform);
        else
            CanvasUtility.EnemyFieldRect = (transform as RectTransform);
    }

    private void OnTransformChildrenChanged()
    {

    }
}
