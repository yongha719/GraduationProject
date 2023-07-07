using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1Unit : UnitCard
{
    protected override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Debug.Break();
        
        Spawn();
    }

    // 특수 능력이 소환인데 C2 경비원을 양쪽에 2개 스폰함
    private void Spawn()
    {
        var fieldViewId = PhotonManager.GetFieldPhotonView(true).ViewID;

        var leftC2 = CardManager.Instance.CardDrawToName("C2", false, fieldViewId);
        leftC2.transform.SetSiblingIndex(transform.GetSiblingIndex());

        var rightC2 = CardManager.Instance.CardDrawToName("C2", false, fieldViewId);
        rightC2.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
    }
}
