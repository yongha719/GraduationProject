using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EMPProduction : MonoBehaviour
{
    private Image img;

    private float growBigTime = 0.8f;
    private float fadeOutTime = 0.5f;
    void Start()
    {
        img = GetComponent<Image>();
        StartCoroutine(IGrowBig());
    }

    private IEnumerator IGrowBig()
    {
        float current = 0;
        float percent = 0;
        Vector3 scale = img.rectTransform.localScale;
        Vector3 startScale = new Vector3(0.1f, 0.1f, 1);
        Vector3 endScale = new Vector3(2, 2, 1);

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / growBigTime;
            scale = Vector3.Lerp(startScale, endScale, percent);

            img.rectTransform.localScale = scale;

            yield return null;
        }
        StartCoroutine(IFadeOut());

        yield break;

    }

    private IEnumerator IFadeOut()
    {
        float current = 0;
        float percent = 0;

        Color color = img.color;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeOutTime;

            color.a = Mathf.Lerp(1, 0, percent);

            img.color = color;
            yield return null;
        }

        yield break;
    }
}
