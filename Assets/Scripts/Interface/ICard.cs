using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


 /// <summary> Card 중 Unit Card의 Interface </summary>
public interface IUnitCard : IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
     /// <summary> Unit을 치유할 때 호출 </summary>
    public void Heal(int amountofheal);

     /// <summary> Unit이 맞았을 때 호출 </summary>
    public void Hit(int damage);
}
