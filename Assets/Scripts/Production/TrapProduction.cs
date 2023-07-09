using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProduction : MonoBehaviour
{
    private float lerpTime = 0.3f;


    private Vector3 startScale;
    

    void Start()
    {
        StartCoroutine(ISmalling());
    }

    private IEnumerator ISmalling()
    {
        float current = 0;
        float percent = 0;

        Vector3 scale;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        yield return new WaitForSeconds(2.5f);

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / lerpTime;
            scale = Vector3.Lerp(startScale, endScale, percent);
            transform.localScale = scale;

            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }

}
