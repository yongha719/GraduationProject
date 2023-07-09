using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoncannonBoom : MonoBehaviour
{
    private RectTransform rt;
    private Image img;

    private float lerpTime = 0.2f;
    private float fadeOutTime = 0.3f;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        StartCoroutine(IBoom());
    }

    private IEnumerator IBoom()
    {
        float current = 0;
        float percent = 0;
        Vector3 scale;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = new Vector3(10, 10, 1);

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / lerpTime;
            scale = Vector3.Lerp(startScale, endScale, percent);

            rt.localScale = scale;

            yield return null;
        }

        current = 0;
        percent = 0;

        Color color = img.color;
        while(percent< 1)
        {
            current += Time.deltaTime;
            percent = current / fadeOutTime;
            color.a = Mathf.Lerp(1, 0, percent);
            img.color = color;

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
