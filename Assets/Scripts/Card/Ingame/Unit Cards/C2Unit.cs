using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEditor.Experimental;
using UnityEngine;

public class C2Unit : UnitCard
{
    protected override void Start()
    {
        CardState = CardState.Field;
        
        cardInfo.OnFieldStateChange();
    }
}
