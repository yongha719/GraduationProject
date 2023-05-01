using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonPunCallbacks<CardManager>
{
    // 필드에 낼 수 있는 카드가 최대 10개임
    public List<UnitCard> PlayerUnits = new List<UnitCard>(10);
    public List<UnitCard> EnemyUnits = new List<UnitCard>(10);
    
    // 포톤은 오브젝트를 리소스 폴더에서 가져와서 오브젝트 이름으로 저장했음
    private List<string> myDeck = new List<string>();
    /// <summary> 내 덱 </summary>
    public List<string> MyDeck
    {
        get => myDeck;

        set
        {
            myDeck = value;
        }
    }


}
