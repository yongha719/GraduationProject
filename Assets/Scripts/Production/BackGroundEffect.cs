using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundEffect : MonoBehaviour
{
    public List<GameObject> backGroundEffectList = new List<GameObject>();

    [SerializeField]
    private int currentIndex;

    [SerializeField]
    private int postIndex;

    public bool isRun;

    [Range(0, 0.1f)]
    public float interval;

    private void Start()
    {
        StartCoroutine(nameof(Routine));
    }

    private IEnumerator Routine()
    {
        while (isRun)
        {
            RandomGameObject(currentIndex);

            yield return new WaitForSeconds(interval);
        }
    }

    private void RandomGameObject(int currentIndex)
    {
        backGroundEffectList[currentIndex].SetActive(false);
        backGroundEffectList[RandomExcludeIndex(currentIndex)].SetActive(true);
    }

    /// <summary>
    /// 현재 Index를 제외한 숫자를 뽑는 랜덤함수
    /// </summary>
    /// <param name="excludeIndex">제외할 Index</param>
    /// <returns></returns>
    private int RandomExcludeIndex(int excludeIndex)
    {
        int randNum = 0;
        do
        {
            randNum = Random.Range(0, backGroundEffectList.Count);
        } while (randNum == excludeIndex);

        currentIndex= randNum;
        return randNum;
    }

}
