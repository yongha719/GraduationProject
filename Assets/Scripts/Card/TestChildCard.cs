using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChildCard : UnitCard, IUnitCard
{
    protected override void Start()
    {
        base.Start();
    }

    public void Print()
    {
        print("dadaw");
    }
}
