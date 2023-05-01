using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardFieldLayout : MonoBehaviour
{
    [SerializeField]
    private bool IsMine;

    public List<UnitCard> Cards = new List<UnitCard>();

    private void OnTransformChildrenChanged()
    {
        
    }
}
