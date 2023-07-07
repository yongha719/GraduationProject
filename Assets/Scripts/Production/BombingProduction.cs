using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombingProduction : MonoBehaviour
{
    [SerializeField] private Image shadowImage;
    [SerializeField] private GameObject bombEffect;

    [SerializeField] private Vector2 shadowStartPos;
    [SerializeField] private Vector2 shadowEndPos;

    [SerializeField] private float shadowMoveTime;


    public IEnumerator IStartBombing(Transform[] targets)
    {
        float current = 0;
        float percent = 0;
        Vector2 currentPos = shadowImage.transform.position;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / shadowMoveTime;

            currentPos = Vector3.Lerp(shadowStartPos, shadowEndPos, percent);
            shadowImage.transform.position = currentPos;
            yield return null;
        }

        StartCoroutine(IBombing(targets));

        yield break;
    }

    private IEnumerator IBombing(Transform[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            GameObject bomb = Instantiate(bombEffect, targets[i]);
            Destroy(bomb, 2f);
            yield return new WaitForSeconds(0.6f);
        }

        yield break;
    }
}
