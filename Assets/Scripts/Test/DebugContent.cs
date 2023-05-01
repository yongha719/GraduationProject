using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugContent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    public void SetLog(string message)
    {
        Text.text = message;
    }
}
