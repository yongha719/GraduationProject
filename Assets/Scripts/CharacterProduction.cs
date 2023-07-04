using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterProductionResources", menuName = "Productions", order = int.MinValue)]
public class CharacterProductionResoures : ScriptableObject
{
    public List<Sprite> effectBackGround = new List<Sprite>();
    public List<Sprite> effectFront = new List<Sprite>();
    public Sprite illust;
    public Sprite illustBack;
    public Sprite characterName;
}

public class CharacterProduction : MonoBehaviour
{
    public enum ECharacterType
    {
        Baekyura,
        
    }

    [SerializeField] private Transform effectBackGround;
    [SerializeField] private Transform effectFront;
    [SerializeField] private Transform illust;
    [SerializeField] private Transform illustBack;
    [SerializeField] private Transform characterName;

    [SerializeField] private SpriteRenderer darkBackGround;

    [SerializeField] private List<SpriteRenderer> effectBackGroundSpriteRendererList = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> effectFrontSpriteRendererList = new List<SpriteRenderer>();
    [SerializeField] private SpriteRenderer illustRenderer;
    [SerializeField] private SpriteRenderer illustBackRenderer;
    [SerializeField] private SpriteRenderer characterNameRenderer;

    [SerializeField] private CharacterProductionResoures characterResources;


    float lerpTime = 0.1f;
    #region FrontEffectPos
    private Vector2 startFrontEffectPos = new Vector2(10, 6);
    private Vector2 middleFrontEffectPos = new Vector3(4, 3);
    private Vector2 middleLerpFrontPos = new Vector2(3, 2);
    private Vector2 endFrontPos = new Vector3(-8, -4);
    #endregion
    #region BackEffectPos
    private Vector2 startBackEffectPos = new Vector2(12, 6);
    private Vector2 middleBackEffectPos = new Vector2(2, 1);
    private Vector2 middleLerpBackEffectPos = new Vector2(1, 0);
    private Vector2 endBackEffectPos = new Vector2(-12, -8);
    #endregion

    #region NameEffectPos
    private Vector2 startNameEffectPos = new Vector2(8, 3);
    private Vector2 middleNameEffectPos = new Vector2(1.4f, -0.2f);
    private Vector2 middleLerpNameEffectPos = new Vector2(0.8f, -0.75f);
    private Vector2 endNameEffectPos = new Vector2(-2, -3);
    #endregion

    #region IllustEffectPos
    private Vector2 startillustEffectPos = new Vector2(15, 9);
    private Vector2 middleillustEffectPos = new Vector2(0.25f, 0.25f);
    private Vector2 middleLerpillustEffectPos = new Vector2(-0.25f, -0.25f);
    private Vector2 endillustEffectPos = new Vector2(-14, -7);
    #endregion

    #region IllustBackEffectPos
    private Vector2 startIllustBackEffectPos = new Vector2(15, 9);
    private Vector2 middleIllustBackEffectPos = new Vector2(1, 0.25f);
    private Vector2 middleLerpIllustBackEffectPos = new Vector2(-0.25f, -0.25f);
    private Vector2 endIllustBackEffectPos = new Vector2(-14, -7);
    #endregion

    private Vector2 originPos = new Vector2(32, 17);

    private void Start()
    {
        StartCoroutine(IPlay2());
        darkBackGround.GetComponent<Transform>().position = Vector2.zero;
    }

    private void LoadReSource()
    {
        for (int i = 0; i < 3; i++)
        {
            effectBackGroundSpriteRendererList[i].sprite = characterResources.effectBackGround[i];
            effectFrontSpriteRendererList[i].sprite = characterResources.effectFront[i];
        }
        
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

            origin.localPosition = currentPos;
            yield return null;
        }

        yield break;
    }
    private IEnumerator IFadeInOutImage(Image image, float startAlpha, float endAlpha, float time)
    {
        float current = 0;
        float percent = 0;

        Color fadeColor = default;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, percent);

            image.color = fadeColor;

            yield return null;

        }
        yield break;
    }
    private IEnumerator IFadeInOutSprite(SpriteRenderer sprite, float startAlpha, float endAlpha, float time)
    {
        float current = 0;
        float percent = 0;

        Color fadeColor = default;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, percent);

            sprite.color = fadeColor;

            yield return null;

        }
        yield break;
    }

    private IEnumerator IPlay2()
    {
        StartCoroutine(ILerpTransform(effectFront, startFrontEffectPos, middleFrontEffectPos, lerpTime));
        StartCoroutine(ILerpTransform(effectFront, middleFrontEffectPos, middleLerpFrontPos, 3f));

        StartCoroutine(ILerpTransform(effectBackGround, startBackEffectPos, middleBackEffectPos, lerpTime));
        StartCoroutine(ILerpTransform(effectBackGround, middleBackEffectPos, middleLerpBackEffectPos, 3f));

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ILerpTransform(illust, startillustEffectPos, middleillustEffectPos, lerpTime));

        StartCoroutine(ILerpTransform(characterName, startNameEffectPos, middleNameEffectPos, lerpTime));

        yield return new WaitForSeconds(lerpTime);
        StartCoroutine(ILerpTransform(illust, middleillustEffectPos, middleLerpIllustBackEffectPos, 2f));

        StartCoroutine(ILerpTransform(characterName, middleNameEffectPos, middleLerpNameEffectPos, 2f));

        StartCoroutine(IFadeInOutSprite(darkBackGround, 0, 1, 0.5f));

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ILerpTransform(illustBack, startIllustBackEffectPos, middleIllustBackEffectPos, lerpTime));

        yield return new WaitForSeconds(lerpTime);

        StartCoroutine(ILerpTransform(illustBack, middleIllustBackEffectPos, middleLerpIllustBackEffectPos, 1.5f));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ILerpTransform(effectFront, middleLerpFrontPos, endFrontPos, lerpTime));
        StartCoroutine(ILerpTransform(effectBackGround, middleLerpBackEffectPos, endBackEffectPos, 0.1f));
        StartCoroutine(ILerpTransform(illust, middleLerpillustEffectPos, endillustEffectPos, lerpTime));
        StartCoroutine(ILerpTransform(characterName, middleLerpNameEffectPos, endNameEffectPos, lerpTime));
        StartCoroutine(ILerpTransform(illustBack, middleLerpIllustBackEffectPos, endIllustBackEffectPos, lerpTime));
        StartCoroutine(IFadeInOutSprite(darkBackGround, 1, 0, 0.5f));

        yield return new WaitForSeconds(0.1f);
        effectFront.gameObject.SetActive(false);
        effectBackGround.gameObject.SetActive(false);

        yield break;
    }
}
