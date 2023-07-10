using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HypothermiaProduction : MonoBehaviour
{
    [SerializeField] private Image img;
    private float loopTime = 0.5f;


    void Start()
    {
        StartCoroutine(IPlay());
    }

    private IEnumerator IPlay()
    {
        float current = 0;
        float percent = 0;
        Color color = img.color;

        while (true)
        {
            current = 0;
            percent = 0;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / loopTime;
                color.a = Mathf.Lerp(0.75f, 0.25f, percent);
                img.color = color;
                yield return null;
            }

            current = 0;
            percent = 0;

            while(percent < 1)
            {
                current += Time.deltaTime;
                percent = current / loopTime;
                color.a = Mathf.Lerp(0.25f, 0.75f, percent);
                img.color = color;
                yield return null;
            }
            yield return null;
        }

        yield break;
    }
}
