using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Photon.Pun;
using UnityEngine;

public class C3Unit : UnitCard
{
    // 한번 버텼는지 체크
    private bool isSurvivedOnce = false;
    
    // 피 1 남기고 한번 버틸 수 있음
    public override void Hit(int damage)
    {
        if (isSurvivedOnce == false && Hp <= damage)
        {
            Hp = 1;
            isSurvivedOnce = true;
        }
        else
            base.Hit(damage);
    }
    
    public override void Hit(int damage, Action<int> hitAction)
    {
        hitAction(Damage);

        Hit(damage);
    }

}