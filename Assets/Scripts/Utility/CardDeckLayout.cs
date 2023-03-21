using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("MyComponent/CardLayout", int.MinValue)]
public class CardDeckLayout : MonoBehaviour
{
    Vector3 leftPosition = new Vector3(-200, -30, 0);
    Vector3 rightPosition = new Vector3(420, -30, 0);
    Quaternion leftRotation = Quaternion.Euler(0, 0, 15f);
    Quaternion rightRotation = Quaternion.Euler(0, 0, -15f);

    List<(Vector3 Pos, Quaternion Rot)> targetPosAndRot = new List<(Vector3 Pos, Quaternion Rot)>();

    public GameObject card;

    private void Start()
    {
        SetPosAndRot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(card, transform);
        }
    }

    private void OnTransformChildrenChanged()
    {
        SetPosAndRot();
    }

    void SetPosAndRot()
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

        targetPosAndRot.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 targetPos = Vector3.Lerp(leftPosition, rightPosition, lerpValue[i]);
            Quaternion targetRos = Quaternion.identity;

            if (transform.childCount >= 4)
            {
                print(Mathf.Pow(lerpValue[i] - 0.5f, 2));
                float curve = Mathf.Sqrt(Mathf.Pow(-0.5f, 2f) - Mathf.Pow(lerpValue[i] - 0.5f, 2)) * 100;
                print($"curve {curve}");
                targetPos.y += curve;
                targetRos = Quaternion.Slerp(leftRotation, rightRotation, lerpValue[i]);
            }
            targetPosAndRot.Add((targetPos, targetRos));
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rect = transform.GetChild(i) as RectTransform;
            print(rect.name);
            rect.anchoredPosition = targetPosAndRot[i].Pos;
            rect.localRotation = targetPosAndRot[i].Rot;
        }
    }
}
