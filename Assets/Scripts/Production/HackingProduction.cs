using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingProduction : MonoBehaviour
{
    [SerializeField]
    private Image img;

    private float fadeOutTime = 0.5f;

    private void Start()
    {
        img = GetComponent<Image>();
        HackingAppearProduction();
    }

    private void HackingAppearProduction()
    {
        StartCoroutine(IHackingAppearProduction());
    }
    private IEnumerator IHackingAppearProduction()
    {
        float current = 0;
        float percent = 0;

        Vector3 startScale = Vector3.one;
        Vector3 endScale = new Vector3(2, 2, 2);
        Color color = img.color;
        Vector3 scale = img.rectTransform.localScale;

        while (percent < 1)
        {
            current = Time.deltaTime;
            percent = current / fadeOutTime;
            scale = Vector3.Lerp(startScale, endScale, percent);
            color.a = Mathf.Lerp(1, 0, percent);

            img.rectTransform.localScale = scale;
            img.color = color;

            yield return null;
        }

        img.rectTransform.localScale = Vector3.one;
        img.color = Color.white;

        yield break;
    }
}