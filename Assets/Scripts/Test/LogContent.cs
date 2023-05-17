using TMPro;
using UnityEngine;

public class LogContent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    private void Awake()
    {
        Text.text = LogManager.Message;
    }
}
