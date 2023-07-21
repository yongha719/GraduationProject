using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCannonProduction : MonoBehaviour
{
    private RectTransform po;

    public GameObject boom;

    private float lerpTime = 1.5f;

    void Start()
    {
        po = GetComponent<RectTransform>();
    }

    public IEnumerator IShotPo(Vector3 targetPos)
    {
        float current = 0;
        float percent = 0;
        Vector3 startPos = new Vector3(0, -550f, 0);
        Vector3 endPos = targetPos;
        float spd = 1;

        while (percent < 1)
        {
            current += Time.deltaTime * spd;
            percent = current / lerpTime;

            Vector3 pos = Vector3.Lerp(startPos, endPos, percent);

            po.anchoredPosition = pos;

            spd += 0.05f;
            yield return null;
        }

        Instantiate(boom, transform.parent);

        yield break;
    }
}
