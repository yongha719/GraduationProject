using Photon.Pun;
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

    public GameObject card;

    private void Start()
    {
        SetPosAndRot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && card != null)
        {
            Instantiate(card, transform);
            GameObject testcard = PhotonNetwork.Instantiate("Prefabs/Deck_Card", Vector2.zero, Quaternion.identity);
            testcard.transform.parent = transform;
            testcard.transform.localScale = Vector3.one;
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

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 targetPos = Vector3.Lerp(leftPosition, rightPosition, lerpValue[i]);
            Quaternion targetRos = Quaternion.identity;

            if (transform.childCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(0.5f, 2f) - Mathf.Pow(lerpValue[i] - 0.5f, 2)) * 80f;
                targetPos.y += curve;
                targetRos = Quaternion.Slerp(leftRotation, rightRotation, lerpValue[i]);
            }

            RectTransform rect = transform.GetChild(i) as RectTransform;
            rect.anchoredPosition = targetPos;
            rect.localRotation = targetRos;
        }
    }
}
