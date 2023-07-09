using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastBreathProduction : MonoBehaviour
{
    private float enlargementTime = 0.2f;

    [SerializeField] private Image img;
    void Start()
    {
        StartCoroutine(IPlay());
    }

    private IEnumerator IPlay()
    {
        float current = 0;
        float percent = 0;
        Vector3 scale;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(1f, 1f, 1);

        Vector3 scale1 = new Vector3(1.4f, 1.4f, 1);

        Vector3 scale2 = new Vector3(1.2f, 1.2f, 1);

        Color color = img.color;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / enlargementTime;
            scale = Vector3.Lerp(startScale, scale1, percent);
            img.GetComponent<RectTransform>().localScale = scale;

            yield return null;
        }

        yield return new WaitForSeconds(0.05f);
        current = 0;
        percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / (enlargementTime * 0.7f);
            scale = Vector3.Lerp(scale1, endScale, percent);
            img.GetComponent<RectTransform>().localScale = scale;

            yield return null;
        }
        yield return new WaitForSeconds(0.05f);

        current = 0;
        percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / (enlargementTime * 0.7f);
            scale = Vector3.Lerp(endScale, scale2, percent);
            img.GetComponent<RectTransform>().localScale = scale;

            yield return null;
        }



        current = 0;
        percent = 0;

        for (int i = 0; i < 8; i++)
        {
            if (i % 2 == 0)
                color.a = 1;
            else
                color.a = 0.75f;
            img.color = color;
            yield return new WaitForSeconds(0.3f);
        }

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / enlargementTime;
            scale = Vector3.Lerp(scale1, startScale, percent);
            img.GetComponent<RectTransform>().localScale = scale;

            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }

}
