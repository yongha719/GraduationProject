using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Commander : MonoBehaviour
{
    private int hp;

    public int Hp
    {
        get => hp;

        set
        {
            if (value <= 0)
                isWin = false;
                
            hp = value;
            
        }
    }

    private bool isWin = false; 
    
    public bool CanAttackThis => CardManager.Instance.PlayerUnitCards.Count == 0;

    [PunRPC]
    private void GameOver()
    {
        
    }
}
