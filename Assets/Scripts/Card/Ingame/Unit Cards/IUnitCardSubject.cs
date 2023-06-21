using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitCardSubject
{
    void HealCard(int healAmount);

    void Hit(int damage);
    void Hit(int damage, Action<int> hitAction);

    void Destroy();
}
