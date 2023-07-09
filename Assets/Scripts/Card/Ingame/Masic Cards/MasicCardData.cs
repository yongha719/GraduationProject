using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection.Configuration;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public enum MasicAbilityTarget
{
    Field,
    Ally,
    Enemy,
}

[Serializable]
public class MasicCardData
{
    [Tooltip("카드 이름")] 
    private string name;

    public string Name => name;

    [Tooltip("타겟 타입")]
    private MasicAbilityTarget masicAbilityTarget;
    
    public MasicAbilityTarget MasicAbilityTarget=> masicAbilityTarget;

    [Tooltip("코스트")]
    private uint cost;
    
    public uint Cost => cost;

    [Tooltip("카드 등급")] 
    private string cardRating;

    public string CardRating => cardRating;
    
    
}
