using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// PhotonNetwork Reference
// https://doc-api.photonengine.com/ko-kr/pun/current/class_photon_network.html

// MonoBehaviourPunCallbacks Reference
// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("¾È³ç ³» ÀÌ¸§ ±è½Ã¿ø");


    }
}
