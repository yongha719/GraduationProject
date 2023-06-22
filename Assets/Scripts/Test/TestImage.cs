using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TestImage : MonoBehaviour
{
    void Start()
    {
        var image = GetComponent<Image>();

        Color color = Color.black;

        ColorUtility.TryParseHtmlString("#7E4949", out color);

        image.color = color;
    }
}
