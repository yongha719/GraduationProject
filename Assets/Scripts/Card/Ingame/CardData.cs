using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CardData
{
    [field: SerializeField]
    public virtual string Name { get; protected set; }

    [field: SerializeField]
    public virtual int Cost { get; protected set; }

    [field: SerializeField]
    public virtual string CardRating { get; protected set; }

    public abstract void Init(string[] data);

    public abstract CardData Copy();
}