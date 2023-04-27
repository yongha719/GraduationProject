using System;
using UnityEngine;

[Serializable]
public class TurnChangeButton : MonoBehaviour
{
    public void TurnChange() => TurnManager.Instance.TurnChange();
}
