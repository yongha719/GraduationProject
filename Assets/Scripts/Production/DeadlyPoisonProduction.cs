using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadlyPoisonProduction : MonoBehaviour
{
    private Image img;

    [SerializeField] private float cycle;
    void Start()
    {
        img = GetComponent<Image>();
        StartCoroutine(DeadlyPoison());
    }

    /// <summary>
    /// 맹독 연출 코루틴 (기본적으로 무한 반복)
    /// </summary>
    /// <param name="cycle">깜박거림 주기</param>
    /// <returns></returns>
    private IEnumerator DeadlyPoison()
    {
        float current = 0;
        float percent = 0;
        Color color = img.color;
        while (gameObject.active)
        {
            current = 0;
            percent = 0;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (cycle / 2);
                color.a = Mathf.Lerp(0.15f, 0.8f, percent);
                img.color = color;

                yield return null;
            }

            current = 0;
            percent = 0;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (cycle / 2);
                color.a = Mathf.Lerp(0.8f, 0.15f, percent);
                img.color = color;

                yield return null;
            }
            
            yield return null;
        }

        yield break;
    }
}
