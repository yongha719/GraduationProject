using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyComponent/CardLayout", int.MinValue)]
public class CardLayout : MonoBehaviour
{
    private void OnTransformChildrenChanged()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var rect = transform.GetChild(i) as RectTransform;

            rect.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * i * 2 / transform.childCount));
        }
    }
}
