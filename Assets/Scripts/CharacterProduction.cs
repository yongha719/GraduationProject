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
    [SerializeField] private Vector3 endPos2;


    private IEnumerator IPlay1()
    {
        StartCoroutine(IFadeInOut(darkBackGround, 0, 1, 0.2f));

        #region 화면 밖에서 화면중앙으로
        StartCoroutine(ILerp(glitering, startPos1, middlePos, 0.1f));
        StartCoroutine(ILerp(backGround, startPos1, middlePos, 0.1f));

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ILerp(illustration, startPos1, middlePos, 0.1f));

        yield return new WaitForSeconds(0.05f);

        StartCoroutine(ILerp(illustration_back, startPos1, middlePos, 0.1f));
        #endregion
        yield return new WaitForSeconds(2f);

        #region 화면중앙에서 화면 밖으로
        StartCoroutine(ILerp(illustration, middlePos, endPos1, 0.1f));
        StartCoroutine(ILerp(illustration_back, middlePos, endPos1, 0.1f));

        StartCoroutine(IFadeInOut(darkBackGround, 1, 0, 0.2f));
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ILerp(glitering, middlePos, endPos1, 0.1f));

        StartCoroutine(ILerp(backGround, middlePos, endPos1, 0.1f));

        #endregion

        Destroy(gameObject);
        yield break;
    }

    private IEnumerator ILerp(RectTransform origin, Vector3 startPos, Vector2 endPos, float time)
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
        yield return null;
    }

    private IEnumerator ISlowLerp(RectTransform origin, Vector2 startPos,Vector2 endPos, float timeSpd, float time)
    {


        while(0 < time)
        {

            yield return null;
        }

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
