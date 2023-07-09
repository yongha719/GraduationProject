using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageTextProduction : MonoBehaviour
{
    float lerpTime = 0.5f;

    [SerializeField] private TextMeshProUGUI damageText;

    [SerializeField] private Image damageImg;
    [SerializeField] private Image healingImg;

    private void Start()
    {
        SetDamage(5);
    }

    public void SetDamage(int damage, bool isHeal = false)
    {
        if (isHeal == false)
        {
            damageText.text = $"-{damage}";
            damageImg.gameObject.SetActive(true);
            healingImg.gameObject.SetActive(false);
        }
        else
        {
            damageText.text = $"+{damage}";
            damageImg.gameObject.SetActive(false);
            healingImg.gameObject.SetActive(true);
        }

        StartCoroutine(IScaleBig());
    }
    private IEnumerator IScaleBig()
    {
        float current = 0;
        float percent = 0;
        Vector3 scale;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / lerpTime;
            scale = Vector3.Lerp(startScale, endScale, percent);
            transform.localScale = scale;

            yield return null;
        }

        yield return new WaitForSeconds(1);
        current = 0;
        percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / lerpTime;
            scale = Vector3.Lerp(endScale, startScale, percent);
            transform.localScale = scale;

            yield return null;
        }
        yield break;
    }

}
