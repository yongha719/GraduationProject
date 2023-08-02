using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitCardSubject
{
    public bool IsHacked { set; }
    
    void HealCard(int healAmount);

    void Hit(int damage);

    void Hit(int damage, Action<int> hitAction);

    /// <summary>
    /// 매 턴마다 실행
    /// </summary>
    void HandleTurn();

    void Destroy();
}
