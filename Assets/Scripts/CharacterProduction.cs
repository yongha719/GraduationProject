using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProduction : MonoBehaviour
{
    [SerializeField] private RectTransform glitering;
    [SerializeField] private RectTransform backGround;
    [SerializeField] private RectTransform illustration;
    [SerializeField] private RectTransform illustration_back;

    [SerializeField] private Image darkBackGround;


    [SerializeField] private Vector2 startPos1;
    private Vector2 middlePos = Vector3.zero;
    [SerializeField] private Vector2 endPos1;

    [SerializeField] private Vector2 startPos2;
    private Vector2 middleLerpPos = new Vector2(-0.5f, -0.5f);
    [SerializeField] private Vector2 endPos2;


    private IEnumerator IPlay1()
    {
        StartCoroutine(IFadeInOut(darkBackGround, 0, 1, 0.2f));

        #region 화면 밖에서 화면중앙으로
        StartCoroutine(ILerpRectTransform(glitering, startPos1, middlePos, 0.1f));
        StartCoroutine(ILerpRectTransform(backGround, startPos1, middlePos, 0.1f));

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ILerpRectTransform(illustration, startPos1, middlePos, 0.1f));

        yield return new WaitForSeconds(0.05f);

        StartCoroutine(ILerpRectTransform(illustration_back, startPos1, middlePos, 0.1f));
        #endregion
        yield return new WaitForSeconds(2f);

        #region 화면중앙에서 화면 밖으로
        StartCoroutine(ILerpRectTransform(illustration, middlePos, endPos1, 0.1f));
        StartCoroutine(ILerpRectTransform(illustration_back, middlePos, endPos1, 0.1f));

        StartCoroutine(IFadeInOut(darkBackGround, 1, 0, 0.2f));
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ILerpRectTransform(glitering, middlePos, endPos1, 0.1f));

        StartCoroutine(ILerpRectTransform(backGround, middlePos, endPos1, 0.1f));

        #endregion

        Destroy(gameObject);
        yield break;
    }

    private IEnumerator ILerpRectTransform(RectTransform origin, Vector3 startPos, Vector2 endPos, float time)
    {
        float current = 0;
        float percent = 0;

        Vector3 currentPos;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            currentPos = Vector2.Lerp(startPos, endPos, percent);

            origin.anchoredPosition = currentPos;
            yield return null;
        }

        yield break;
    }

    private IEnumerator ILerpTransform(Transform origin, Vector3 startPos, Vector3 endPos, float time)
    {
        float current = 0;
        float percent = 0;

        Vector3 currentPos;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            currentPos = Vector2.Lerp(startPos, endPos, percent);

            origin.position = currentPos;
            yield return null;
        }

        yield break;
    }
    private IEnumerator IFadeInOut(Image image, float startAlpha, float endAlpha, float time)
    {
        float current = 0;
        float percent = 0;

        Color fadeColor = default;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, percent);

            image.color = fadeColor;

            yield return null;

        }
        yield break;
    }

    private IEnumerator IPlay2()
    {
        StartCoroutine(ILerpTransform(transform, startPos2, middlePos, 0.1f));
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ILerpTransform(transform, middlePos, middleLerpPos, 2f));
        yield return new WaitForSeconds(2f);

        StartCoroutine(ILerpTransform(transform, middleLerpPos, endPos2, 0.1f));
        yield return new WaitForSeconds(0.1f);

        yield break;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(IPlay1());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(IPlay2());
        }
    }
}
