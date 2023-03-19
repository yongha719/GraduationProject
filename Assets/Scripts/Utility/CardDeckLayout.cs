using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyComponent/CardLayout", int.MinValue)]
public class CardDeckLayout : MonoBehaviour
{
    Vector3 distance = new Vector3(300, 0, 0);
    Quaternion leftRotation = Quaternion.Euler(new Vector3(0, 0, 15));
    Quaternion rightRotation = Quaternion.Euler(new Vector3(0, 0, -15));

    private void OnTransformChildrenChanged()
    {
        float[] lerpValue = new float[transform.childCount];

        switch (transform.childCount)
        {
            case 1:
                lerpValue = new float[] { 0.5f };
                break;
            case 2:
                lerpValue = new float[] { 0.27f, 0.73f };
                break;
            case 3:
                lerpValue = new float[] { 0.1f, 0.5f, 0.9f };
                break;
            default:
                float interval = 1f / (transform.childCount - 1);
                for (int i = 0; i < transform.childCount; i++)
                    lerpValue[i] = interval * i;
                break;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 targetPos = Vector3.Lerp(-distance, distance, lerpValue[i]);
            Quaternion targetRos = Quaternion.identity;

            if (transform.childCount >= 4)
            {
                print(transform.childCount);
                float curve = Mathf.Sqrt(Mathf.Pow(0.5f, 2) - Mathf.Pow(lerpValue[i] - 0.5f, 2));
                targetPos.y += curve;
                targetRos = Quaternion.Slerp(leftRotation, rightRotation, lerpValue[i]);
            }

            RectTransform rect = transform.GetChild(i) as RectTransform;
            print(rect.name);
            print(targetPos);
            rect.anchoredPosition = targetPos;
            rect.localRotation = targetRos;
        }
    }
}
