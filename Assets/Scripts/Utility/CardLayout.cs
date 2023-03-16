using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyComponent/CardLayout", int.MinValue)]
public class CardLayout : MonoBehaviour
{
    private void OnTransformChildrenChanged()
    {
        print(transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
        {
            float angle = i * 120 / transform.childCount;

            transform.GetChild(i).rotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg);
        }
    }
}
