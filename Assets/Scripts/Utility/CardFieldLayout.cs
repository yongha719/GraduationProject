using Photon.Pun;
using UnityEngine;

public class CardFieldLayout : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.ViewID == (int)PhotonViewType.PlayerField)
            CanvasUtility.MyFieldRect = transform as RectTransform;
        else
            CanvasUtility.EnemyFieldRect = transform as RectTransform;
    }
}