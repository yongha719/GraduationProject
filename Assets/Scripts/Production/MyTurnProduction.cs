using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTurnProduction : MonoBehaviour
{
    private float smallingTime = 0.5f;

    private void Start()
    {
        StartCoroutine(ISmalling());
    }

    private IEnumerator ISmalling()
    {
        float current = 0;
        float percent = 0;
        Vector3 scale = transform.localScale;
        Vector3 startScale = new Vector3(1, 1, 1);
        Vector3 endScale = new Vector3(0, 0, 1);

        yield return new WaitForSeconds(2f);

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / smallingTime;
            scale = Vector3.Lerp(startScale, endScale, percent);

            transform.localScale = scale;

            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }

}
