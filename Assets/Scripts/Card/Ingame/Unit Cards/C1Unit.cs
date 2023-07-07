using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1Unit : UnitCard
{
    protected override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Spawn();
    }

    // 특수 능력이 소환인데 C2 경비원을 양쪽에 2개 스폰함
    private void Spawn()
    {
        var slibingindex = transform.GetSiblingIndex();

        var leftC2 = CardManager.Instance.CardDrawToName("C2");
        leftC2.transform.SetSiblingIndex(slibingindex == 0 ? 0 : slibingindex - 1);

        var rightC2 = CardManager.Instance.CardDrawToName("C2");
        rightC2.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
    }
}
