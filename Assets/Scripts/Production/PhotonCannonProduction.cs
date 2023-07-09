using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCannonProduction : MonoBehaviour
{
    [SerializeField] private RectTransform po;

    private float lerpTime = 0.3f;
    void Start()
    {
        
    }




    private IEnumerator IShotPo(Vector3 targetPos)
    {
        float current = 0;
        float percent = 0;
        Vector3 startPos = new Vector3(0, -550f, 0);
        Vector3 endPos = targetPos;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / lerpTime;

            


            yield return null;
        }
        
        


        yield break;
    }
}
