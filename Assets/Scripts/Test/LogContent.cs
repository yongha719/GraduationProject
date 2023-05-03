using TMPro;
using UnityEngine;

public class LogContent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    public void SetLog(string message)
    {
        Text.text = message;
    }
}
