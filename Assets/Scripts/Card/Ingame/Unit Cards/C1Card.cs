using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1Card : UnitCard
{
    public override void MoveCardFromDeckToField()
    {
        base.MoveCardFromDeckToField();

        Instantiate(illustAppearEffect).GetComponent<CharacterProduction>().characterType = ECharacterType.Leesooha;

        SoundManager.Instance.PlaySFXSound(ECharacterType.Leesooha.ToString());
        Spawn();
    }

    // 특수 능력이 소환인데 C2 경비원을 양쪽에 2개 스폰함
    private void Spawn()
    {
        var leftC2 =
            CardManager.Instance.CardDraw("C2", setParentAsDeck: false);
        leftC2.transform.SetSiblingIndex(transform.GetSiblingIndex());

        var rightC2 =
            CardManager.Instance.CardDraw("C2", setParentAsDeck: false);
        rightC2.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

        SoundManager.Instance.PlaySFXSound("EndySpawn");
    }
}