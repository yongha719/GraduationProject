using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IUnitCard : IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public void Heal(int amountofheal);
    public void Hit(int damage);
}
