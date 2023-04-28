using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>, IPunObservable
{
    void Start()
    {
        MyDebug.Log("Test Log");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SerializeUnitCards();
    }
}
