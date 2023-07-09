using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShot : MonoBehaviour
{
    private void Start()
    {
        
    }

    private IEnumerator IMissileShot(Transform[] targets)
    {
        float current = 0;
        float percent = 0;
        Vector2 startPos = Vector2.zero;

        for (int i = 0; i < targets.Length; i++)
        {

        }

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / Time.deltaTime;




        }

        yield break;
    }
}
